// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SettingsViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.ViewModels {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.ViewModelBase;
    using FFXIVAPP.Plugin.Event.Properties;
    using FFXIVAPP.Plugin.Event.Views;

    internal sealed class SettingsViewModel : INotifyPropertyChanged {
        private static Lazy<SettingsViewModel> _instance = new Lazy<SettingsViewModel>(() => new SettingsViewModel());

        public SettingsViewModel() {
            this.TestSoundCommand = new DelegateCommand(TestSound);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static SettingsViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand TestSoundCommand { get; private set; }

        private static void TestSound() {
            if (SettingsView.View.TSound.Text.Trim() == string.Empty) {
                return;
            }

            var volume = Settings.Default.GlobalVolume * 100;
            SoundPlayerHelper.PlayCached(SettingsView.View.TSound.Text, (int) volume);
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}