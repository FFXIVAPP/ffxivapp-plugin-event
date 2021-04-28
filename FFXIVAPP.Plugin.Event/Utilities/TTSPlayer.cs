// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TTSPlayer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   TTSPlayer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Utilities {
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Speech.Synthesis;

    using FFXIVAPP.Common;

    using NAudio.Wave;

    public static class TTSPlayer {
        private const int Latency = 100;

        private static readonly ConcurrentDictionary<TTSData, byte[]> Cached;

        static TTSPlayer() {
            Cached = new ConcurrentDictionary<TTSData, byte[]>();
        }

        public static void Speak(string tts, int volume, int rate) {
            var ttsData = new TTSData {
                Text = tts,
                Volume = volume,
                Rate = rate,
            };
            var disposable = new Disposable {
                MemoryStream = GetMemoryStream(ttsData),
                DirectSoundOut = Constants.DefaultAudioDevice == Guid.Empty
                                     ? new DirectSoundOut(Latency)
                                     : new DirectSoundOut(Constants.DefaultAudioDevice, Latency),
            };
            disposable.WaveFileReader = new WaveFileReader(disposable.MemoryStream);
            disposable.WaveChannel = new WaveChannel32(disposable.WaveFileReader);
            disposable.DirectSoundOut.Init(disposable.WaveChannel);
            disposable.DirectSoundOut.PlaybackStopped += disposable.DisposeDirectSound;
            disposable.DirectSoundOut.Play();
        }

        private static byte[] CreateNewMemoryStream(TTSData ttsData) {
            using (var memstream = new MemoryStream()) {
                using (var synthesizer = new SpeechSynthesizer()) {
                    synthesizer.SetOutputToWaveStream(memstream);
                    synthesizer.Volume = ttsData.Volume;
                    synthesizer.Rate = ttsData.Rate;

                    synthesizer.Speak(ttsData.Text);
                }

                memstream.Seek(0, SeekOrigin.Begin);
                return memstream.ToArray();
            }
        }

        private static MemoryStream GetMemoryStream(TTSData ttsData) {
            byte[] ttsBytes = Cached.GetOrAdd(ttsData, t => CreateNewMemoryStream(ttsData));
            return new MemoryStream(ttsBytes);
        }

        private class Disposable : IDisposable {
            public DirectSoundOut DirectSoundOut;

            public MemoryStream MemoryStream;

            public WaveChannel32 WaveChannel;

            public WaveFileReader WaveFileReader;

            public void Dispose() {
                if (this.WaveFileReader != null) {
                    this.WaveFileReader.Dispose();
                }

                this.WaveFileReader = null;

                if (this.WaveChannel != null) {
                    this.WaveChannel.Dispose();
                }

                this.WaveChannel = null;

                if (this.MemoryStream != null) {
                    this.MemoryStream.Dispose();
                }

                this.MemoryStream = null;

                if (this.DirectSoundOut != null) {
                    this.DirectSoundOut.Dispose();
                }

                this.DirectSoundOut = null;
            }

            public void DisposeDirectSound(object sender, StoppedEventArgs e) {
                this.DirectSoundOut.PlaybackStopped -= this.DisposeDirectSound;
                this.Dispose();
            }
        }

        private class TTSData : IEquatable<TTSData> {
            public TTSData() {
                this.Rate = -2;
                this.Volume = 100;
            }

            public int Rate { get; set; }

            public string Text { get; set; }

            public int Volume { get; set; }

            public static bool operator ==(TTSData left, TTSData right) {
                return Equals(left, right);
            }

            public static bool operator !=(TTSData left, TTSData right) {
                return !Equals(left, right);
            }

            public bool Equals(TTSData other) {
                if (ReferenceEquals(null, other)) {
                    return false;
                }

                if (ReferenceEquals(this, other)) {
                    return true;
                }

                return string.Equals(this.Text, other.Text, StringComparison.OrdinalIgnoreCase) && this.Volume == other.Volume && this.Rate == other.Rate;
            }

            public override bool Equals(object obj) {
                if (ReferenceEquals(null, obj)) {
                    return false;
                }

                if (ReferenceEquals(this, obj)) {
                    return true;
                }

                if (obj.GetType() != this.GetType()) {
                    return false;
                }

                return this.Equals((TTSData) obj);
            }

            public override int GetHashCode() {
                return this.Text == null
                           ? 0
                           : this.Text.GetHashCode();
            }
        }
    }
}