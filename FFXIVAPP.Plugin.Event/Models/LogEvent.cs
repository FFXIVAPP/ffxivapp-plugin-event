// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEvent.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LogEvent.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Models {
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class LogEvent : INotifyPropertyChanged {
        private string _arguments;

        private string _category;

        private int _delay;

        private bool _enabled;

        private string _executable;

        private Guid _key;

        private int _rate;

        private string _regEx;

        private string _sound;

        private string _tts;

        private double _volume;

        public LogEvent() {
            this.Delay = 0;
            this.Enabled = true;
            this.Executable = string.Empty;
            this.Sound = string.Empty;
            this.Volume = 100;
            this.TTS = string.Empty;
            this.Rate = -2;
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public string Arguments {
            get {
                return this._arguments;
            }

            set {
                this._arguments = value;
                this.RaisePropertyChanged();
            }
        }

        public string Category {
            get {
                return this._category;
            }

            set {
                this._category = value;
                this.RaisePropertyChanged();
            }
        }

        public int Delay {
            get {
                return this._delay;
            }

            set {
                this._delay = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Enabled {
            get {
                return this._enabled;
            }

            set {
                this._enabled = value;
                this.RaisePropertyChanged();
            }
        }

        public string Executable {
            get {
                return this._executable;
            }

            set {
                this._executable = value;
                this.RaisePropertyChanged();
            }
        }

        public Guid Key {
            get {
                return this._key;
            }

            set {
                this._key = value;
                this.RaisePropertyChanged();
            }
        }

        public int Rate {
            get {
                return this._rate;
            }

            set {
                this._rate = value;
                this.RaisePropertyChanged();
            }
        }

        public string RegEx {
            get {
                return this._regEx;
            }

            set {
                this._regEx = value;
                this.RaisePropertyChanged();
            }
        }

        public string Sound {
            get {
                return this._sound;
            }

            set {
                this._sound = value;
                this.RaisePropertyChanged();
            }
        }

        public string TTS {
            get {
                return this._tts;
            }

            set {
                this._tts = value;
                this.RaisePropertyChanged();
            }
        }

        public double Volume {
            get {
                return this._volume;
            }

            set {
                this._volume = value;
                this.RaisePropertyChanged();
            }
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }
    }
}