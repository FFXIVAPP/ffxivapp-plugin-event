// FFXIVAPP.Plugin.Event ~ TTSPlayer.cs
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
using System.Collections.Concurrent;
using System.IO;
using System.Speech.Synthesis;
using NAudio.Wave;

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class TTSPlayer
    {
        private const int Latency = 100;
        private static readonly ConcurrentDictionary<TTSData, byte[]> Cached;

        static TTSPlayer()
        {
            Cached = new ConcurrentDictionary<TTSData, byte[]>();
        }

        public static void Speak(string tts, int volume, int rate)
        {
            var ttsData = new TTSData
            {
                Text = tts,
                Volume = volume,
                Rate = rate
            };
            var disposable = new Disposable
            {
                MemoryStream = GetMemoryStream(ttsData),
                DirectSoundOut = (Common.Constants.DefaultAudioDevice == Guid.Empty) ? new DirectSoundOut(Latency) : new DirectSoundOut(Common.Constants.DefaultAudioDevice, Latency)
            };
            disposable.WaveFileReader = new WaveFileReader(disposable.MemoryStream);
            disposable.WaveChannel = new WaveChannel32(disposable.WaveFileReader);
            disposable.DirectSoundOut.Init(disposable.WaveChannel);
            disposable.DirectSoundOut.PlaybackStopped += disposable.DisposeDirectSound;
            disposable.DirectSoundOut.Play();
        }

        private static MemoryStream GetMemoryStream(TTSData ttsData)
        {
            var ttsBytes = Cached.GetOrAdd(ttsData, t => CreateNewMemoryStream(ttsData));
            return new MemoryStream(ttsBytes);
        }

        private static byte[] CreateNewMemoryStream(TTSData ttsData)
        {
            using (var memstream = new MemoryStream())
            {
                using (var synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.SetOutputToWaveStream(memstream);
                    synthesizer.Volume = ttsData.Volume;
                    synthesizer.Rate = ttsData.Rate;

                    synthesizer.Speak(ttsData.Text);
                }
                memstream.Seek(0, SeekOrigin.Begin);
                return memstream.ToArray();
            }
        }

        private class Disposable : IDisposable
        {
            public DirectSoundOut DirectSoundOut;
            public MemoryStream MemoryStream;
            public WaveChannel32 WaveChannel;
            public WaveFileReader WaveFileReader;

            public void Dispose()
            {
                if (WaveFileReader != null)
                {
                    WaveFileReader.Dispose();
                }
                WaveFileReader = null;

                if (WaveChannel != null)
                {
                    WaveChannel.Dispose();
                }
                WaveChannel = null;

                if (MemoryStream != null)
                {
                    MemoryStream.Dispose();
                }
                MemoryStream = null;

                if (DirectSoundOut != null)
                {
                    DirectSoundOut.Dispose();
                }
                DirectSoundOut = null;
            }

            public void DisposeDirectSound(object sender, StoppedEventArgs e)
            {
                DirectSoundOut.PlaybackStopped -= DisposeDirectSound;
                Dispose();
            }
        }

        private class TTSData : IEquatable<TTSData>
        {
            public TTSData()
            {
                Rate = -2;
                Volume = 100;
            }

            public string Text { get; }
            public int Volume { get; set; }
            public int Rate { get; set; }

            public bool Equals(TTSData other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }
                if (ReferenceEquals(this, other))
                {
                    return true;
                }
                return string.Equals(Text, other.Text, StringComparison.OrdinalIgnoreCase) && Volume == other.Volume && Rate == other.Rate;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                if (ReferenceEquals(this, obj))
                {
                    return true;
                }
                if (obj.GetType() != GetType())
                {
                    return false;
                }
                return Equals((TTSData) obj);
            }

            public override int GetHashCode()
            {
                return (Text == null) ? 0 : Text.GetHashCode();
            }

            public static bool operator ==(TTSData left, TTSData right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(TTSData left, TTSData right)
            {
                return !Equals(left, right);
            }
        }
    }
}
