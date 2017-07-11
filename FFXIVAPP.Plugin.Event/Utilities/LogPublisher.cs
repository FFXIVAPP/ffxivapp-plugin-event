// FFXIVAPP.Plugin.Event ~ LogPublisher.cs
// 
// Copyright © 2007 - 2017 Ryan Wilson - All Rights Reserved
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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Event.Models;
using FFXIVAPP.Plugin.Event.Properties;
using NLog;
using Sharlayan.Core;

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class LogPublisher
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

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
                        resuccess = item.RegEx == line;
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
                Logging.Log(Logger, new LogItem(ex, true));
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
