// FFXIVAPP.Plugin.Event ~ ExecutableHelper.cs
// 
// Copyright © 2007 - 2016 Ryan Wilson - All Rights Reserved
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
using System.Diagnostics;
using System.IO;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Plugin.Event.Utilities
{
    public static class ExecutableHelper
    {
        public static void Run(string path, string arguments)
        {
            if (String.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return;
            }
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    Arguments = (string.IsNullOrWhiteSpace(arguments) ? "" : arguments),
                    FileName = path
                };
                using (Process.Start(processStartInfo))
                {
                    ;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "Executable failed.", ex);
            }
        }
    }
}
