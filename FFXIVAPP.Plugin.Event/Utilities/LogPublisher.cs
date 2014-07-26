// FFXIVAPP.Plugin.Event
// LogPublisher.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
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
                foreach (var item in PluginViewModel.Instance.Events)
                {
                    if (!item.Enabled)
                    {
                        continue;
                    }
                    var resuccess = false;
                    var check = new Regex(item.RegEx);
                    if (SharedRegEx.IsValidRegex(item.RegEx))
                    {
                        var reg = check.Match(line);
                        if (reg.Success)
                        {
                            resuccess = true;
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
                    PlaySound(item);
                    RunExecutable(item);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
        }

        private static void PlaySound(LogEvent logEvent)
        {
            if (String.IsNullOrWhiteSpace(logEvent.Sound))
            {
                return;
            }
            var delay = logEvent.Delay;
            Func<bool> playSound = () =>
            {
                var timer = new Timer(delay > 0 ? delay * 1000 : 1);
                ElapsedEventHandler timerEventHandler = null;
                timerEventHandler = delegate
                {
                    DispatcherHelper.Invoke(delegate
                    {
                        var volume = logEvent.Volume * Settings.Default.GlobalVolume;
                        SoundPlayerHelper.PlayCached(logEvent.Sound, (int) volume);
                    });
                    timer.Elapsed -= timerEventHandler;
                };
                timer.Elapsed += timerEventHandler;
                timer.Start();
                return true;
            };
            playSound.BeginInvoke(null, null);
        }

        private static void RunExecutable(LogEvent logEvent)
        {
            if (String.IsNullOrWhiteSpace(logEvent.Executable))
            {
                return;
            }

            var delay = logEvent.Delay;
            if (delay <= 0)
            {
                ExecutableHelper.Run(logEvent.Executable, logEvent.Arguments);
            }
            else
            {
                var timer = new Timer(delay*1000);
                ElapsedEventHandler timerOnElapsed = null;
                timerOnElapsed = 
                    delegate
                    {
                        timer.Elapsed -= timerOnElapsed;
                        timer.Dispose();
                        ExecutableHelper.Run(logEvent.Executable, logEvent.Arguments);
                    };
                timer.Elapsed += timerOnElapsed;
                timer.Start();
            }
        }
    }
}
