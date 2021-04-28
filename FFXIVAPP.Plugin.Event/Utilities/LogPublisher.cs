// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPublisher.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   LogPublisher.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Utilities {
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

    public static class LogPublisher {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Process(ChatLogItem chatLogItem) {
            try {
                var line = chatLogItem.Line.Replace("  ", " ");
                foreach (LogEvent item in PluginViewModel.Instance.Events.Where(e => e.Enabled)) {
                    var resuccess = false;
                    var arguments = item.Arguments;
                    var tts = item.TTS;
                    if (SharedRegEx.IsValidRegex(item.RegEx)) {
                        Match reg = Regex.Match(line, item.RegEx);
                        if (reg.Success) {
                            resuccess = true;
                            arguments = reg.Result(item.Arguments);
                            tts = reg.Result(tts);
                        }
                    }
                    else {
                        resuccess = item.RegEx == line;
                    }

                    if (!resuccess) {
                        continue;
                    }

                    ExecutLogEvent(item, arguments, tts);
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private static void ExecuteActions(IEnumerable<Action> actions) {
            foreach (Action action in actions) {
                action();
            }
        }

        private static void ExecutLogEvent(LogEvent logEvent, string arguments, string tts) {
            var volume = Convert.ToInt32(logEvent.Volume * Settings.Default.GlobalVolume);

            List<Action> actions = new List<Action> {
                PlaySound(logEvent, volume),
                RunExecutable(logEvent, arguments),
                PlayTTS(tts, volume, logEvent.Rate),
            };
            actions.RemoveAll(a => a == null);
            if (!actions.Any()) {
                return;
            }

            var delay = logEvent.Delay;
            if (delay <= 0) {
                ExecuteActions(actions);
            }
            else {
                var timer = new Timer(delay * 1000);
                ElapsedEventHandler timerEventHandler = null;
                timerEventHandler = delegate {
                    timer.Elapsed -= timerEventHandler;
                    timer.Dispose();

                    ExecuteActions(actions);
                };
                timer.Elapsed += timerEventHandler;
                timer.Start();
            }
        }

        private static Action PlaySound(LogEvent logEvent, int volume) {
            var soundFile = logEvent.Sound;
            if (string.IsNullOrWhiteSpace(soundFile)) {
                return null;
            }

            return () => SoundPlayerHelper.PlayCached(soundFile, volume);
        }

        private static Action PlayTTS(string tts, int volume, int rate) {
            if (string.IsNullOrWhiteSpace(tts)) {
                return null;
            }

            return () => TTSPlayer.Speak(tts, volume, rate);
        }

        private static Action RunExecutable(LogEvent logEvent, string arguments) {
            if (string.IsNullOrWhiteSpace(logEvent.Executable)) {
                return null;
            }

            return () => ExecutableHelper.Run(logEvent.Executable, arguments);
        }
    }
}