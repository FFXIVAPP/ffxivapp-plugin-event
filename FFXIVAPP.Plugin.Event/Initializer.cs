// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Initializer.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Initializer.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Event.Models;
    using FFXIVAPP.Plugin.Event.Properties;
    using FFXIVAPP.Plugin.Event.ViewModels;

    using NLog;

    internal static class Initializer {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void ApplyTheming() {
            MainViewModel.SetupGrouping();
        }

        public static void LoadLogEvents() {
            const string defaultCategoryLabel = "event_MiscellaneousLabel";
            var defaultCategory = PluginViewModel.Instance.Locale.ContainsKey(defaultCategoryLabel)
                                      ? PluginViewModel.Instance.Locale[defaultCategoryLabel]
                                      : "Miscellaneous";

            PluginViewModel.Instance.Events.Clear();
            if (Constants.XSettings != null) {
                foreach (XElement xElement in Constants.XSettings.Descendants().Elements("Event")) {
                    // migrate regex from key, if necessary
                    var xRegEx = xElement.Element("RegEx") != null
                                     ? (string) xElement.Element("RegEx")
                                     : (string) xElement.Attribute("Key");
                    if (string.IsNullOrWhiteSpace(xRegEx)) {
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
                    Guid xKey = xElement.GetAttributeValue("Key", Guid.NewGuid());
                    xSound = string.IsNullOrWhiteSpace(xValue)
                                 ? xSound
                                 : xValue;
                    var logEvent = new LogEvent {
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
                    if (!found) {
                        PluginViewModel.Instance.Events.Add(logEvent);
                    }
                }
            }
        }

        public static void LoadSettings() {
            if (Constants.XSettings != null) {
                Settings.Default.Reset();
                foreach (XElement xElement in Constants.XSettings.Descendants().Elements("Setting")) {
                    var xKey = (string) xElement.Attribute("Key");
                    var xValue = (string) xElement.Element("Value");
                    if (string.IsNullOrWhiteSpace(xKey) || string.IsNullOrWhiteSpace(xValue)) {
                        continue;
                    }

                    if (Constants.Settings.Contains(xKey)) {
                        Settings.Default.SetValue(xKey, xValue, CultureInfo.InvariantCulture);
                    }
                    else {
                        Constants.Settings.Add(xKey);
                    }
                }
            }
        }

        public static void LoadSoundsAndCache() {
            PluginViewModel.Instance.SoundFiles.Clear();

            // do your gui stuff here
            List<FileInfo> legacyFiles = new List<FileInfo>();
            List<string> filters = new List<string> {
                "*.wav",
                "*.mp3",
            };
            foreach (var filter in filters) {
                IEnumerable<FileInfo> files = Directory.GetFiles(Constants.BaseDirectory, filter, SearchOption.AllDirectories).Select(file => new FileInfo(file));
                legacyFiles.AddRange(files);
            }

            foreach (FileInfo legacyFile in legacyFiles) {
                if (legacyFile.DirectoryName == null) {
                    continue;
                }

                var baseKey = legacyFile.DirectoryName.Replace(Constants.BaseDirectory, string.Empty);
                var key = string.IsNullOrWhiteSpace(baseKey)
                              ? legacyFile.Name
                              : $"{baseKey.Substring(1)}\\{legacyFile.Name}";
                if (File.Exists(Path.Combine(Common.Constants.SoundsPath, key))) {
                    continue;
                }

                try {
                    var directoryKey = string.IsNullOrWhiteSpace(baseKey)
                                           ? string.Empty
                                           : baseKey.Substring(1);
                    var directory = Path.Combine(Common.Constants.SoundsPath, directoryKey);
                    if (!Directory.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }

                    File.Copy(legacyFile.FullName, Path.Combine(Common.Constants.SoundsPath, key));
                    SoundPlayerHelper.TryGetSetSoundFile(key);
                }
                catch (Exception ex) {
                    Logging.Log(Logger, new LogItem(ex, true));
                }
            }

            foreach (var cachedSoundFile in SoundPlayerHelper.SoundFileKeys()) {
                PluginViewModel.Instance.SoundFiles.Add(cachedSoundFile);
            }

            PluginViewModel.Instance.SoundFiles.Insert(0, " ");
        }

        private static T DeserializeValue<T>(string value, T defaultValue) {
            if (string.IsNullOrEmpty(value)) {
                return defaultValue;
            }

            try {
                return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
            }
            catch (Exception) {
                return defaultValue;
            }
        }

        private static T GetAttributeValue<T>(this XElement xElement, string attributeName, T defaultvalue) {
            XAttribute xAttribute = xElement.Attribute(attributeName);
            if (xAttribute == null) {
                return defaultvalue;
            }

            return DeserializeValue(xAttribute.Value, defaultvalue);
        }

        private static T GetElementValue<T>(this XContainer xElement, string childElementName, T defaultvalue) {
            XElement childElement = xElement.Element(childElementName);
            if (childElement == null) {
                return defaultvalue;
            }

            return DeserializeValue(childElement.Value, defaultvalue);
        }
    }
}