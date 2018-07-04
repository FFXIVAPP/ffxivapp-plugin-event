// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShellViewModel.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ShellViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using FFXIVAPP.Plugin.Event.Properties;

    public sealed class ShellViewModel : INotifyPropertyChanged {
        private static Lazy<ShellViewModel> _instance = new Lazy<ShellViewModel>(() => new ShellViewModel());

        public ShellViewModel() {
            Initializer.LoadSettings();
            Initializer.LoadLogEvents();
            Settings.Default.PropertyChanged += DefaultOnPropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static ShellViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        internal static void Loaded(object sender, RoutedEventArgs e) {
            ShellView.View.Loaded -= Loaded;
            Initializer.ApplyTheming();
            Initializer.LoadSoundsAndCache();
        }

        private static void DefaultOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs) { }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}