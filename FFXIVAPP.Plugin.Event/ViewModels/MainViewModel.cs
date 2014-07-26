// FFXIVAPP.Plugin.Event
// MainViewModel.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

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
using FFXIVAPP.Plugin.Event.Views;
using Microsoft.Win32;
using NLog;

namespace FFXIVAPP.Plugin.Event.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
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
            if (cvEvents != null && cvEvents.CanGroup == true)
            {
                cvEvents.GroupDescriptions.Clear();
                cvEvents.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            }
        }

        #region Loading Functions

        #endregion

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
            if (MainView.View.TSound.Text.Trim() == "")
            {
                return;
            }
            var volume = (MainView.View.TVolume.Value * 100) * Settings.Default.GlobalVolume;
            SoundPlayerHelper.PlayCached(MainView.View.TSound.Text, (int) volume);
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
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
            MainView.View.TRegEx.Text = "";
            MainView.View.TExecutable.Text = "";
            MainView.View.TArguments.Text = "";
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
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
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
            MainView.View.TVolume.Value = Convert.ToDouble(GetValueBySelectedItem(MainView.View.Events, "Volume")) / 100;
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
            var name = matches.Groups["category"].Value;
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
            var category = matches.Groups["category"].Value;
            var events = new List<LogEvent>(PluginViewModel.Instance.Events.ToList());
            var enabledCount = events.Where(@event => @event.Category == category)
                                     .Count(@event => @event.Enabled);
            var enable = enabledCount == 0 || (enabledCount < events.Count(@event => @event.Category == category));
            if (enable)
            {
                for (var i = 0; i < events.Count; i++)
                {
                    if (events[i].Category == category)
                    {
                        PluginViewModel.Instance.Events[i].Enabled = true;
                    }
                }
            }
            else
            {
                for (var i = 0; i < events.Count; i++)
                {
                    if (events[i].Category == category)
                    {
                        PluginViewModel.Instance.Events[i].Enabled = false;
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
