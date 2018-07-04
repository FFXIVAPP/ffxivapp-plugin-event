// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   MainViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.ViewModels {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
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

    internal sealed class MainViewModel : INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Lazy<MainViewModel> _instance = new Lazy<MainViewModel>(() => new MainViewModel());

        public MainViewModel() {
            this.RefreshSoundListCommand = new DelegateCommand(RefreshSoundList);
            this.TestSoundCommand = new DelegateCommand(TestSound);
            this.AddOrUpdateEventCommand = new DelegateCommand(AddOrUpdateEvent);
            this.DeleteEventCommand = new DelegateCommand(DeleteEvent);
            this.EventSelectionCommand = new DelegateCommand(EventSelection);
            this.DeleteCategoryCommand = new DelegateCommand<string>(DeleteCategory);
            this.ToggleCategoryCommand = new DelegateCommand<string>(ToggleCategory);
            this.SelectExecutableCommand = new DelegateCommand(SelectExecutable);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static MainViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand AddOrUpdateEventCommand { get; private set; }

        public ICommand DeleteCategoryCommand { get; private set; }

        public ICommand DeleteEventCommand { get; private set; }

        public ICommand EventSelectionCommand { get; private set; }

        public ICommand RefreshSoundListCommand { get; private set; }

        public ICommand SelectExecutableCommand { get; private set; }

        public ICommand TestSoundCommand { get; private set; }

        public ICommand ToggleCategoryCommand { get; private set; }

        public static void SetupGrouping() {
            ICollectionView cvEvents = CollectionViewSource.GetDefaultView(MainView.View.Events.ItemsSource);
            if (cvEvents != null && cvEvents.CanGroup) {
                cvEvents.GroupDescriptions.Clear();
                cvEvents.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            }
        }

        /// <summary>
        /// </summary>
        private static void AddOrUpdateEvent() {
            Guid selectedId = Guid.Empty;
            try {
                if (MainView.View.Events.SelectedItems.Count == 1) {
                    selectedId = new Guid(GetValueBySelectedItem(MainView.View.Events, "Key"));
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            if (string.IsNullOrWhiteSpace(MainView.View.TDelay.Text) || string.IsNullOrWhiteSpace(MainView.View.TRegEx.Text)) {
                return;
            }

            if (string.IsNullOrWhiteSpace(MainView.View.TCategory.Text)) {
                MainView.View.TCategory.Text = PluginViewModel.Instance.Locale["event_MiscellaneousLabel"];
            }

            if (Regex.IsMatch(MainView.View.TDelay.Text, @"[^0-9]+")) {
                var popupContent = new PopupContent {
                    PluginName = Plugin.PName,
                    Title = PluginViewModel.Instance.Locale["app_WarningMessage"],
                    Message = "Delay can only be numeric."
                };
                Plugin.PHost.PopupMessage(Plugin.PName, popupContent);
                return;
            }

            var logEvent = new LogEvent {
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
            if (int.TryParse(MainView.View.TDelay.Text, out result)) {
                logEvent.Delay = result;
            }

            if (selectedId == Guid.Empty) {
                logEvent.Key = Guid.NewGuid();
                PluginViewModel.Instance.Events.Add(logEvent);
            }
            else {
                logEvent.Key = selectedId;
                var index = PluginViewModel.Instance.Events.TakeWhile(@event => @event.Key != selectedId).Count();
                PluginViewModel.Instance.Events[index] = logEvent;
            }

            MainView.View.Events.UnselectAll();
            MainView.View.TRegEx.Text = string.Empty;
            MainView.View.TTTS.Text = string.Empty;
            MainView.View.TExecutable.Text = string.Empty;
            MainView.View.TArguments.Text = string.Empty;
        }

        private static void DeleteCategory(string categoryName) {
            var categoryRegEx = new Regex(@"(?<category>.+) \(\d+\)", SharedRegEx.DefaultOptions);
            Match matches = categoryRegEx.Match(categoryName);
            if (!matches.Success) {
                return;
            }

            var name = matches.Groups["category"].Value;
            List<LogEvent> events = new List<LogEvent>(PluginViewModel.Instance.Events.ToList());
            foreach (LogEvent @event in events.Where(@event => @event.Category == name)) {
                PluginViewModel.Instance.Events.Remove(@event);
            }
        }

        /// <summary>
        /// </summary>
        private static void DeleteEvent() {
            string selectedKey;
            try {
                selectedKey = GetValueBySelectedItem(MainView.View.Events, "Key");
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
                return;
            }

            var index = PluginViewModel.Instance.Events.TakeWhile(@event => @event.Key.ToString() != selectedKey).Count();
            PluginViewModel.Instance.Events.RemoveAt(index);
        }

        /// <summary>
        /// </summary>
        private static void EventSelection() {
            if (MainView.View.Events.SelectedItems.Count != 1) {
                return;
            }

            if (MainView.View.Events.SelectedIndex < 0) {
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

        /// <summary>
        /// </summary>
        /// <param name="listView"> </param>
        /// <param name="key"> </param>
        private static string GetValueBySelectedItem(Selector listView, string key) {
            Type type = listView.SelectedItem.GetType();
            PropertyInfo property = type.GetProperty(key);
            return property.GetValue(listView.SelectedItem, null).ToString();
        }

        /// <summary>
        /// </summary>
        private static void RefreshSoundList() {
            Initializer.LoadSoundsAndCache();
            SetupGrouping();
        }

        /// <summary>
        /// </summary>
        private static void SelectExecutable() {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog().GetValueOrDefault()) {
                MainView.View.TExecutable.Text = dialog.FileName;
            }
        }

        private static void TestSound() {
            var volume = Convert.ToInt32(MainView.View.TVolume.Value * 100 * Settings.Default.GlobalVolume);
            if (!string.IsNullOrWhiteSpace(MainView.View.TSound.Text)) {
                SoundPlayerHelper.PlayCached(MainView.View.TSound.Text, volume);
            }

            if (!string.IsNullOrWhiteSpace(MainView.View.TTTS.Text)) {
                TTSPlayer.Speak(MainView.View.TTTS.Text, volume, (int) MainView.View.TRate.Value);
            }
        }

        private static void ToggleCategory(string categoryName) {
            var categoryRegEx = new Regex(@"(?<category>.+) \(\d+\)$", SharedRegEx.DefaultOptions);
            Match matches = categoryRegEx.Match(categoryName);
            if (!matches.Success) {
                return;
            }

            MainView.View.Events.SelectedItem = null;
            var category = matches.Groups["category"].Value;
            List<LogEvent> events = new List<LogEvent>(PluginViewModel.Instance.Events.ToList());
            var enabledCount = events.Where(@event => @event.Category == category).Count(@event => @event.Enabled);
            var enable = enabledCount == 0 || enabledCount < events.Count(@event => @event.Category == category);
            if (enable) {
                for (var i = 0; i < events.Count; i++) {
                    if (events[i].Category == category) {
                        PluginViewModel.Instance.Events[i].Enabled = true;
                    }
                }
            }
            else {
                for (var i = 0; i < events.Count; i++) {
                    if (events[i].Category == category) {
                        PluginViewModel.Instance.Events[i].Enabled = false;
                    }
                }
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}