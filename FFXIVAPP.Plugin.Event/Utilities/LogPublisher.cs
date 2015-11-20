// FFXIVAPP.Plugin.Event
// LogPublisher.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Event.Models;
using FFXIVAPP.Plugin.Event.Properties;
using NLog;

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class LogPublisher
    {
        public static void Process(ChatLogEntry chatLogEntry)
        {
            try
            {
                var line = chatLogEntry.Line.Replace("  ", " ");
                foreach (var item in PluginViewModel.Instance.Events.Where(e => e.Enabled))
                {
                    var resuccess = false;
                    var arguments = item.Arguments;
                    var tts = item.TTS;
                    if (SharedRegEx.IsValidRegex(item.RegEx))
                    {
                        var reg = Regex.Match(line, item.RegEx);
                        if (reg.Success)
                        {
                            resuccess = true;
                            arguments = reg.Result(item.Arguments);
                            tts = reg.Result(tts);
                        }
                    }
                    else
                    {
                        resuccess = (item.RegEx == line);
                    }
                    if (!resuccess)
                    {
                        continue;
                    }

                    ExecutLogEvent(item, arguments, tts);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        private static void ExecutLogEvent(LogEvent logEvent, string arguments, string tts)
        {
            var volume = Convert.ToInt32(logEvent.Volume * Settings.Default.GlobalVolume);

            var actions = new List<Action>
            {
                PlaySound(logEvent, volume),
                RunExecutable(logEvent, arguments),
                PlayTTS(tts, volume, logEvent.Rate)
            };
            actions.RemoveAll(a => a == null);
            if (!actions.Any())
            {
                return;
            }

            var delay = logEvent.Delay;
            if (delay <= 0)
            {
                ExecuteActions(actions);
            }
            else
            {
                var timer = new Timer(delay * 1000);
                ElapsedEventHandler timerEventHandler = null;
                timerEventHandler = delegate
                {
                    timer.Elapsed -= timerEventHandler;
                    timer.Dispose();

                    ExecuteActions(actions);
                };
                timer.Elapsed += timerEventHandler;
                timer.Start();
            }
        }

        private static Action PlaySound(LogEvent logEvent, int volume)
        {
            var soundFile = logEvent.Sound;
            if (String.IsNullOrWhiteSpace(soundFile))
            {
                return null;
            }

            return () => SoundPlayerHelper.PlayCached(soundFile, volume);
        }

        private static Action RunExecutable(LogEvent logEvent, string arguments)
        {
            if (String.IsNullOrWhiteSpace(logEvent.Executable))
            {
                return null;
            }

            return () => ExecutableHelper.Run(logEvent.Executable, arguments);
        }

        private static Action PlayTTS(string tts, int volume, int rate)
        {
            if (String.IsNullOrWhiteSpace(tts))
            {
                return null;
            }

            return () => TTSPlayer.Speak(tts, volume, rate);
        }

        private static void ExecuteActions(IEnumerable<Action> actions)
        {
            foreach (var action in actions)
            {
                action();
            }
        }
    }
}
