// FFXIVAPP.Plugin.Event ~ LogEvent.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FFXIVAPP.Plugin.Event.Models
{
    public class LogEvent : INotifyPropertyChanged
    {
        public LogEvent()
        {
            Delay = 0;
            Enabled = true;
            Executable = "";
            Sound = "";
            Volume = 100;
            TTS = "";
            Rate = -2;
        }

        #region Property Bindings

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

        public Guid Key
        {
            get { return _key; }
            set
            {
                _key = value;
                RaisePropertyChanged();
            }
        }

        public string Sound
        {
            get { return _sound; }
            set
            {
                _sound = value;
                RaisePropertyChanged();
            }
        }

        public string TTS
        {
            get { return _tts; }
            set
            {
                _tts = value;
                RaisePropertyChanged();
            }
        }

        public int Rate
        {
            get { return _rate; }
            set
            {
                _rate = value;
                RaisePropertyChanged();
            }
        }

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                RaisePropertyChanged();
            }
        }

        public int Delay
        {
            get { return _delay; }
            set
            {
                _delay = value;
                RaisePropertyChanged();
            }
        }

        public string RegEx
        {
            get { return _regEx; }
            set
            {
                _regEx = value;
                RaisePropertyChanged();
            }
        }

        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                RaisePropertyChanged();
            }
        }

        public string Executable
        {
            get { return _executable; }
            set
            {
                _executable = value;
                RaisePropertyChanged();
            }
        }

        public string Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;
                RaisePropertyChanged();
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                RaisePropertyChanged();
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
