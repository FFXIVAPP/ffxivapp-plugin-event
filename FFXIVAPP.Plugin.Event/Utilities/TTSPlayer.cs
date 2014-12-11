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
        private static readonly ConcurrentDictionary<Tuple<string, int>, byte[]> Cached;

        static TTSPlayer()
        {
            Cached = new ConcurrentDictionary<Tuple<string, int>, byte[]>(new KeyEqualityComparer());
        }

        public static void Speak(string tts, int volume)
        {
            var disposable = new Disposable
                             {
                                 MemoryStream = GetMemoryStream(tts, volume),
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

        private static MemoryStream GetMemoryStream(string tts, int volume)
        {
            var tuple = new Tuple<string, int>(tts, volume);
            var ttsData = Cached.GetOrAdd(tuple, t => CreateNewMemoryStream(t.Item1, t.Item2));
            return new MemoryStream(ttsData);
        }

        private static byte[] CreateNewMemoryStream(string tts, int volume)
        {
            using (var memstream = new MemoryStream())
            {
                using (var synthesizer = new SpeechSynthesizer())
                {
                    synthesizer.SetOutputToWaveStream(memstream);
                    synthesizer.Volume = volume;
                    synthesizer.Rate = -2;

                    synthesizer.Speak(tts);
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

        private class KeyEqualityComparer : IEqualityComparer<Tuple<string, int>>
        {
            public bool Equals(Tuple<string, int> x, Tuple<string, int> y)
            {
                return x.Item2 == y.Item2
                       && string.Equals(x.Item1, y.Item1, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Tuple<string, int> obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}