// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutableHelper.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   ExecutableHelper.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Utilities {
    using System;
    using System.Diagnostics;
    using System.IO;

    using FFXIVAPP.Common.Utilities;

    using NLog;

    public static class ExecutableHelper {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Run(string path, string arguments) {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) {
                return;
            }

            try {
                var processStartInfo = new ProcessStartInfo {
                    Arguments = string.IsNullOrWhiteSpace(arguments)
                                    ? string.Empty
                                    : arguments,
                    FileName = path
                };
                using (Process.Start(processStartInfo)) {
                    ;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, "Executable Failed", ex);
            }
        }
    }
}