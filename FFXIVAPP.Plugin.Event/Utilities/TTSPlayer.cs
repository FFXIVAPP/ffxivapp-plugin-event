using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using FFXIVAPP.Common.Audio;
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
                                 DirectSoundOut = (Common.Constants.DefaultAudioDevice == Guid.Empty)
                                     ? new DirectSoundOut(Latency)
                                     : new DirectSoundOut(Common.Constants.DefaultAudioDevice, Latency),
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
            public WaveFileReader WaveFileReader;
            public WaveChannel32 WaveChannel;
            public MemoryStream MemoryStream;

            public void Dispose()
            {
                if (WaveFileReader != null)
                    WaveFileReader.Dispose();
                WaveFileReader = null;

                if (WaveChannel != null)
                    WaveChannel.Dispose();
                WaveChannel = null;

                if (MemoryStream != null)
                    MemoryStream.Dispose();
                MemoryStream = null;

                if (DirectSoundOut != null)
                    DirectSoundOut.Dispose();
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
            public bool Equals(TTSData other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return string.Equals(Text, other.Text, StringComparison.OrdinalIgnoreCase) && Volume == other.Volume && Rate == other.Rate;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
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

            public string Text { get; set; }
            public int Volume { get; set; }
            public int Rate { get; set; }

            public TTSData()
            {
                Rate = -2;
                Volume = 100;
            }
        }
    }
}