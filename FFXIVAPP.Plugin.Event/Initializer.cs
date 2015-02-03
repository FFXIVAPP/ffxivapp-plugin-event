// FFXIVAPP.Plugin.Event
// Initializer.cs
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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Plugin.Event.Models;
using FFXIVAPP.Plugin.Event.Properties;
using FFXIVAPP.Plugin.Event.ViewModels;

namespace FFXIVAPP.Plugin.Event
{
    internal static class Initializer
    {
        #region Declarations

        #endregion

        public static void LoadSettings()
        {
            if (Constants.XSettings != null)
            {
                Settings.Default.Reset();
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Setting"))
                {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (String.IsNullOrWhiteSpace(xKey) || String.IsNullOrWhiteSpace(xValue))
                    {
                        continue;
                    }
                    if (Constants.Settings.Contains(xKey))
                    {
                        Settings.Default.SetValue(xKey, xValue, CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }

        public static void LoadSoundsAndCache()
        {
            PluginViewModel.Instance.SoundFiles.Clear();
            //do your gui stuff here
            var legacyFiles = new List<FileInfo>();
            var filters = new List<string>
            {
                "*.wav",
                "*.mp3"
            };
            foreach (var filter in filters)
            {
                var files = Directory.GetFiles(Constants.BaseDirectory, filter, SearchOption.AllDirectories)
                                     .Select(file => new FileInfo(file));
                legacyFiles.AddRange(files);
            }
            foreach (var legacyFile in legacyFiles)
            {
                if (legacyFile.DirectoryName == null)
                {
                    continue;
                }
                var baseKey = legacyFile.DirectoryName.Replace(Constants.BaseDirectory, "");
                var key = String.IsNullOrWhiteSpace(baseKey) ? legacyFile.Name : String.Format("{0}\\{1}", baseKey.Substring(1), legacyFile.Name);
                if (File.Exists(Path.Combine(Common.Constants.SoundsPath, key)))
                {
                    continue;
                }
                try
                {
                    var directoryKey = String.IsNullOrWhiteSpace(baseKey) ? "" : baseKey.Substring(1);
                    var directory = Path.Combine(Common.Constants.SoundsPath, directoryKey);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    File.Copy(legacyFile.FullName, Path.Combine(Common.Constants.SoundsPath, key));
                    SoundPlayerHelper.TryGetSetSoundFile(key);
                }
                catch (Exception ex)
                {
                }
            }
            foreach (var cachedSoundFile in SoundPlayerHelper.SoundFileKeys())
            {
                PluginViewModel.Instance.SoundFiles.Add(cachedSoundFile);
            }
            PluginViewModel.Instance.SoundFiles.Insert(0, " ");
        }

        public static void LoadLogEvents()
        {
            const string defaultCategoryLabel = "event_MiscellaneousLabel";
            var defaultCategory = PluginViewModel.Instance.Locale.ContainsKey(defaultCategoryLabel) ? PluginViewModel.Instance.Locale[defaultCategoryLabel] : "Miscellaneous";

            PluginViewModel.Instance.Events.Clear();
            if (Constants.XSettings != null)
            {
                foreach (var xElement in Constants.XSettings.Descendants()
                                                  .Elements("Event"))
                {
                    // migrate regex from key, if necessary
                    var xRegEx = xElement.Element("RegEx") != null ? (string) xElement.Element("RegEx") : (string) xElement.Attribute("Key");
                    if (String.IsNullOrWhiteSpace(xRegEx))
                    {
                        continue;
                    }

                    var xValue = xElement.GetElementValue("Value", string.Empty);
                    var xSound = xElement.GetElementValue("Sound", string.Empty);
                    var xTTS = xElement.GetElementValue("TTS", string.Empty);
                    var xRate = xElement.GetElementValue("Rate", -2);
                    var xVolume = xElement.GetElementValue("Volume", 1.0d);
                    var xDelay = xElement.GetElementValue("Delay", 0);
                    var xCategory = xElement.GetElementValue("Category", defaultCategory);
                    var xExecutable = xElement.GetElementValue("Executable", string.Empty);
                    var xArguments = xElement.GetElementValue("Arguments", string.Empty);
                    var xEnabled = xElement.GetElementValue("Enabled", true);
                    var xKey = xElement.GetAttributeValue("Key", Guid.NewGuid());
                    xSound = String.IsNullOrWhiteSpace(xValue) ? xSound : xValue;
                    var logEvent = new LogEvent
                    {
                        Key = xKey,
                        Sound = xSound,
                        TTS = xTTS,
                        Rate = xRate,
                        Delay = xDelay,
                        Volume = xVolume,
                        RegEx = xRegEx,
                        Category = xCategory,
                        Enabled = xEnabled,
                        Executable = xExecutable,
                        Arguments = xArguments,
                    };
                    var found = PluginViewModel.Instance.Events.Any(@event => @event.Key == logEvent.Key);
                    if (!found)
                    {
                        PluginViewModel.Instance.Events.Add(logEvent);
                    }
                }
            }
        }

        private static T GetElementValue<T>(this XContainer xElement, string childElementName, T defaultvalue)
        {
            var childElement = xElement.Element(childElementName);
            if (childElement == null)
            {
                return defaultvalue;
            }

            return DeserializeValue(childElement.Value, defaultvalue);
        }

        private static T GetAttributeValue<T>(this XElement xElement, string attributeName, T defaultvalue)
        {
            var xAttribute = xElement.Attribute(attributeName);
            if (xAttribute == null)
            {
                return defaultvalue;
            }

            return DeserializeValue(xAttribute.Value, defaultvalue);
        }

        private static T DeserializeValue<T>(string value, T defaultValue)
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }
            try
            {
                return (T) TypeDescriptor.GetConverter(typeof (T))
                                         .ConvertFromInvariantString(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static void ApplyTheming()
        {
            MainViewModel.SetupGrouping();
        }
    }
}
