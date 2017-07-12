// FFXIVAPP.Plugin.Event ~ MainViewModel.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Event.Models;
using FFXIVAPP.Plugin.Event.Properties;
using FFXIVAPP.Plugin.Event.Utilities;
using FFXIVAPP.Plugin.Event.Views;
using Microsoft.Win32;
using NLog;

namespace FFXIVAPP.Plugin.Event.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public MainViewModel()
        {
            RefreshSoundListCommand = new DelegateCommand(RefreshSoundList);
            TestSoundCommand = new DelegateCommand(TestSound);
            AddOrUpdateEventCommand = new DelegateCommand(AddOrUpdateEvent);
            DeleteEventCommand = new DelegateCommand(DeleteEvent);
            EventSelectionCommand = new DelegateCommand(EventSelection);
            DeleteCategoryCommand = new DelegateCommand<string>(DeleteCategory);
            ToggleCategoryCommand = new DelegateCommand<string>(ToggleCategory);
            SelectExecutableCommand = new DelegateCommand(SelectExecutable);
        }

        public static void SetupGrouping()
        {
            var cvEvents = CollectionViewSource.GetDefaultView(MainView.View.Events.ItemsSource);
            if (cvEvents != null && cvEvents.CanGroup)
            {
                cvEvents.GroupDescriptions.Clear();
                cvEvents.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            }
        }

        #region Utility Functions

        /// <summary>
        /// </summary>
        /// <param name="listView"> </param>
        /// <param name="key"> </param>
        private static string GetValueBySelectedItem(Selector listView, string key)
        {
            var type = listView.SelectedItem.GetType();
            var property = type.GetProperty(key);
            return property.GetValue(listView.SelectedItem, null)
                           .ToString();
        }

        #endregion

        #region Property Bindings

        private static Lazy<MainViewModel> _instance = new Lazy<MainViewModel>(() => new MainViewModel());

        public static MainViewModel Instance
        {
            get { return _instance.Value; }
        }

        #endregion

        #region Declarations

        public ICommand RefreshSoundListCommand { get; private set; }
        public ICommand TestSoundCommand { get; private set; }
        public ICommand AddOrUpdateEventCommand { get; private set; }
        public ICommand DeleteEventCommand { get; private set; }
        public ICommand EventSelectionCommand { get; private set; }
        public ICommand DeleteCategoryCommand { get; private set; }
        public ICommand ToggleCategoryCommand { get; private set; }
        public ICommand SelectExecutableCommand { get; private set; }

        #endregion

        #region Loading Functions

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private static void RefreshSoundList()
        {
            Initializer.LoadSoundsAndCache();
            SetupGrouping();
        }

        private static void TestSound()
        {
            var volume = Convert.ToInt32(MainView.View.TVolume.Value * 100 * Settings.Default.GlobalVolume);
            if (!string.IsNullOrWhiteSpace(MainView.View.TSound.Text))
            {
                SoundPlayerHelper.PlayCached(MainView.View.TSound.Text, volume);
            }
            if (!string.IsNullOrWhiteSpace(MainView.View.TTTS.Text))
            {
                TTSPlayer.Speak(MainView.View.TTTS.Text, volume, (int) MainView.View.TRate.Value);
            }
        }

        /// <summary>
        /// </summary>
        private static void AddOrUpdateEvent()
        {
            var selectedId = Guid.Empty;
            try
            {
                if (MainView.View.Events.SelectedItems.Count == 1)
                {
                    selectedId = new Guid(GetValueBySelectedItem(MainView.View.Events, "Key"));
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
            }
            if (string.IsNullOrWhiteSpace(MainView.View.TDelay.Text) || string.IsNullOrWhiteSpace(MainView.View.TRegEx.Text))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(MainView.View.TCategory.Text))
            {
                MainView.View.TCategory.Text = PluginViewModel.Instance.Locale["event_MiscellaneousLabel"];
            }
            if (Regex.IsMatch(MainView.View.TDelay.Text, @"[^0-9]+"))
            {
                var popupContent = new PopupContent
                {
                    PluginName = Plugin.PName,
                    Title = PluginViewModel.Instance.Locale["app_WarningMessage"],
                    Message = "Delay can only be numeric."
                };
                Plugin.PHost.PopupMessage(Plugin.PName, popupContent);
                return;
            }
            var logEvent = new LogEvent
            {
                Sound = MainView.View.TSound.Text,
                TTS = (MainView.View.TTTS.Text ?? string.Empty).Trim(),
                Rate = (int) MainView.View.TRate.Value,
                RegEx = MainView.View.TRegEx.Text,
                Category = MainView.View.TCategory.Text,
                Executable = MainView.View.TExecutable.Text,
                Arguments = MainView.View.TArguments.Text,
                Volume = MainView.View.TVolume.Value * 100
            };
            int result;
            if (Int32.TryParse(MainView.View.TDelay.Text, out result))
            {
                logEvent.Delay = result;
            }
            if (selectedId == Guid.Empty)
            {
                logEvent.Key = Guid.NewGuid();
                PluginViewModel.Instance.Events.Add(logEvent);
            }
            else
            {
                logEvent.Key = selectedId;
                var index = PluginViewModel.Instance.Events.TakeWhile(@event => @event.Key != selectedId)
                                           .Count();
                PluginViewModel.Instance.Events[index] = logEvent;
            }
            MainView.View.Events.UnselectAll();
            MainView.View.TRegEx.Text = string.Empty;
            MainView.View.TTTS.Text = string.Empty;
            MainView.View.TExecutable.Text = string.Empty;
            MainView.View.TArguments.Text = string.Empty;
        }

        /// <summary>
        /// </summary>
        private static void DeleteEvent()
        {
            string selectedKey;
            try
            {
                selectedKey = GetValueBySelectedItem(MainView.View.Events, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, new LogItem(ex, true));
                return;
            }
            var index = PluginViewModel.Instance.Events.TakeWhile(@event => @event.Key.ToString() != selectedKey)
                                       .Count();
            PluginViewModel.Instance.Events.RemoveAt(index);
        }

        /// <summary>
        /// </summary>
        private static void SelectExecutable()
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog()
                      .GetValueOrDefault())
            {
                MainView.View.TExecutable.Text = dialog.FileName;
            }
        }

        /// <summary>
        /// </summary>
        private static void EventSelection()
        {
            if (MainView.View.Events.SelectedItems.Count != 1)
            {
                return;
            }
            if (MainView.View.Events.SelectedIndex < 0)
            {
                return;
            }
            MainView.View.TSound.Text = GetValueBySelectedItem(MainView.View.Events, "Sound");
            MainView.View.TTTS.Text = GetValueBySelectedItem(MainView.View.Events, "TTS");
            MainView.View.TVolume.Value = Convert.ToDouble(GetValueBySelectedItem(MainView.View.Events, "Volume")) / 100;
            MainView.View.TRate.Value = Convert.ToDouble(GetValueBySelectedItem(MainView.View.Events, "Rate"));
            MainView.View.TDelay.Text = GetValueBySelectedItem(MainView.View.Events, "Delay");
            MainView.View.TRegEx.Text = GetValueBySelectedItem(MainView.View.Events, "RegEx");
            MainView.View.TCategory.Text = GetValueBySelectedItem(MainView.View.Events, "Category");
            MainView.View.TExecutable.Text = GetValueBySelectedItem(MainView.View.Events, "Executable");
            MainView.View.TArguments.Text = GetValueBySelectedItem(MainView.View.Events, "Arguments");
        }

        private static void DeleteCategory(string categoryName)
        {
            var categoryRegEx = new Regex(@"(?<category>.+) \(\d+\)", SharedRegEx.DefaultOptions);
            var matches = categoryRegEx.Match(categoryName);
            if (!matches.Success)
            {
                return;
            }
            var name = matches.Groups["category"]
                              .Value;
            var events = new List<LogEvent>(PluginViewModel.Instance.Events.ToList());
            foreach (var @event in events.Where(@event => @event.Category == name))
            {
                PluginViewModel.Instance.Events.Remove(@event);
            }
        }

        private static void ToggleCategory(string categoryName)
        {
            var categoryRegEx = new Regex(@"(?<category>.+) \(\d+\)$", SharedRegEx.DefaultOptions);
            var matches = categoryRegEx.Match(categoryName);
            if (!matches.Success)
            {
                return;
            }
            MainView.View.Events.SelectedItem = null;
            var category = matches.Groups["category"]
                                  .Value;
            var events = new List<LogEvent>(PluginViewModel.Instance.Events.ToList());
            var enabledCount = events.Where(@event => @event.Category == category)
                                     .Count(@event => @event.Enabled);
            var enable = enabledCount == 0 || enabledCount < events.Count(@event => @event.Category == category);
            if (enable)
            {
                for (var i = 0; i < events.Count; i++)
                {
                    if (events[i]
                            .Category == category)
                    {
                        PluginViewModel.Instance.Events[i]
                                       .Enabled = true;
                    }
                }
            }
            else
            {
                for (var i = 0; i < events.Count; i++)
                {
                    if (events[i]
                            .Category == category)
                    {
                        PluginViewModel.Instance.Events[i]
                                       .Enabled = false;
                    }
                }
            }
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
