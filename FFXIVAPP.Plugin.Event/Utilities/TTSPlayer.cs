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
    public class TTSPlayer
    {
        private const int Latency = 100;
        private static readonly ConcurrentDictionary<Tuple<string, int>, byte[]> Cached;

        static TTSPlayer()
        {
            Cached = new ConcurrentDictionary<Tuple<string, int>, byte[]>(new KeyEqualityComparer());
        }

        private DirectSoundOut _directSoundOut;
        private WaveFileReader _waveFileReader;
        private WaveChannel32 _waveChannel;
        private MemoryStream _memoryStream;

        public TTSPlayer()
        {
        }

        public void Speak(string tts, int volume)
        {
            _memoryStream = GetMemoryStream(tts, volume);
            _directSoundOut = (Common.Constants.DefaultAudioDevice == Guid.Empty)
                ? new DirectSoundOut(Latency)
                : new DirectSoundOut(Common.Constants.DefaultAudioDevice, Latency);
            _waveFileReader = new WaveFileReader(_memoryStream);
            _waveChannel = new WaveChannel32(_waveFileReader);
            _directSoundOut.Init(_waveChannel);
            _directSoundOut.Play();
            _directSoundOut.PlaybackStopped += DisposeDirectSound;
        }

        private static MemoryStream GetMemoryStream(string tts, int volume)
        {
            var tuple = new Tuple<string, int>(tts, volume);
            var ttsData = Cached.GetOrAdd(tuple,
                _ =>
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
                });
            return new MemoryStream(ttsData);
        }

        private void DisposeDirectSound(object sender, StoppedEventArgs e)
        {
            _directSoundOut.PlaybackStopped -= DisposeDirectSound;
            Dispose();
        }

        private void Dispose()
        {
            if (_waveChannel != null)
                _waveChannel.Dispose();
            _waveChannel = null;

            if (_waveFileReader != null)
                _waveFileReader.Dispose();
            _waveFileReader = null;

            if (_directSoundOut != null)
                _directSoundOut.Dispose();
            _directSoundOut = null;
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