// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleViewModel.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   SampleViewModel.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.ViewModels {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using FFXIVAPP.Common.ViewModelBase;

    internal sealed class SampleViewModel : INotifyPropertyChanged {
        private static Lazy<SampleViewModel> _instance = new Lazy<SampleViewModel>(() => new SampleViewModel());

        public SampleViewModel() {
            this.SampleCommand = new DelegateCommand(Sample);
            this.SampleCommandT = new DelegateCommand<object>(SampleT);
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static SampleViewModel Instance {
            get {
                return _instance.Value;
            }
        }

        public ICommand SampleCommand { get; private set; }

        public ICommand SampleCommandT { get; private set; }

        /// <summary>
        /// </summary>
        public static void Sample() {
            // do something here
        }

        /// <summary>
        /// </summary>
        public static void SampleT(object parameter) {
            // do something here
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}