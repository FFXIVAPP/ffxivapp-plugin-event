// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Settings.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Properties {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;
    using System.Xml.Linq;

    using FFXIVAPP.Common.Helpers;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.Plugin.Event.Models;

    using NLog;

    using Color = System.Windows.Media.Color;
    using ColorConverter = System.Windows.Media.ColorConverter;
    using FontFamily = System.Drawing.FontFamily;

    public class Settings : ApplicationSettingsBase, INotifyPropertyChanged {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Settings _default;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public static Settings Default {
            get {
                return _default ?? (_default = (Settings) Synchronized(new Settings()));
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("#FF000000")]
        public Color ChatBackgroundColor {
            get {
                return (Color) this["ChatBackgroundColor"];
            }

            set {
                this["ChatBackgroundColor"] = value;
                this.RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("Microsoft Sans Serif, 12pt")]
        public Font ChatFont {
            get {
                return (Font) this["ChatFont"];
            }

            set {
                this["ChatFont"] = value;
                this.RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("1")]
        public double GlobalVolume {
            get {
                return (double) this["GlobalVolume"];
            }

            set {
                this["GlobalVolume"] = value;
                this.RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("#FF800080")]
        public Color TimeStampColor {
            get {
                return (Color) this["TimeStampColor"];
            }

            set {
                this["TimeStampColor"] = value;
                this.RaisePropertyChanged();
            }
        }

        [UserScopedSetting]
        [DebuggerNonUserCode]
        [DefaultSettingValue("100")]
        public double Zoom {
            get {
                return (double) this["Zoom"];
            }

            set {
                this["Zoom"] = value;
                this.RaisePropertyChanged();
            }
        }

        public void Reset() {
            this.DefaultSettings();
            foreach (var key in Constants.Settings) {
                SettingsProperty settingsProperty = Default.Properties[key];
                if (settingsProperty == null) {
                    continue;
                }

                var value = settingsProperty.DefaultValue.ToString();
                this.SetValue(key, value, CultureInfo.InvariantCulture);
            }
        }

        public override void Save() {
            // this call to default settings only ensures we keep the settings we want and delete the ones we don't (old)
            this.DefaultSettings();
            this.SaveSettingsNode();
            this.SaveEventsNode();
            Constants.XSettings.Save(Path.Combine(Common.Constants.PluginsSettingsPath, "FFXIVAPP.Plugin.Event.xml"));
        }

        public void SetValue(string key, string value, CultureInfo cultureInfo) {
            try {
                var type = Default[key].GetType().Name;
                switch (type) {
                    case "Boolean":
                        Default[key] = bool.Parse(value);
                        break;
                    case "Color":
                        var cc = new ColorConverter();
                        object color = cc.ConvertFrom(value);
                        Default[key] = color ?? Colors.Black;
                        break;
                    case "Double":
                        Default[key] = double.Parse(value, cultureInfo);
                        break;
                    case "Font":
                        var fc = new FontConverter();
                        object font = fc.ConvertFromString(value);
                        Default[key] = font ?? new Font(new FontFamily("Microsoft Sans Serif"), 12);
                        break;
                    case "Int32":
                        Default[key] = int.Parse(value, cultureInfo);
                        break;
                    default:
                        Default[key] = value;
                        break;
                }
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }

            this.RaisePropertyChanged(key);
        }

        private void DefaultSettings() {
            Constants.Settings.Clear();
            Constants.Settings.Add("GlobalVolume");
        }

        private void RaisePropertyChanged([CallerMemberName] string caller = "") {
            this.PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        private void SaveEventsNode() {
            if (Constants.XSettings == null) {
                return;
            }

            Constants.XSettings.Descendants("Event").Where(node => PluginViewModel.Instance.Events.All(e => e.Key.ToString() != node.Attribute("Key").Value)).Remove();
            IEnumerable<XElement> xElements = Constants.XSettings.Descendants().Elements("Event");
            XElement[] enumerable = xElements as XElement[] ?? xElements.ToArray();

            foreach (LogEvent item in PluginViewModel.Instance.Events) {
                var xKey = (item.Key != Guid.Empty
                                ? item.Key
                                : Guid.NewGuid()).ToString();
                var xRegEx = item.RegEx;
                var xSound = item.Sound;
                var xTTS = item.TTS;
                var xRate = item.Rate;
                var xVolume = item.Volume;
                var xDelay = item.Delay;
                var xCategory = item.Category;
                var xEnabled = item.Enabled;
                var xExecutable = item.Executable;
                var xArguments = item.Arguments;
                List<XValuePair> keyPairList = new List<XValuePair> {
                    new XValuePair {
                        Key = "RegEx",
                        Value = xRegEx,
                    },
                    new XValuePair {
                        Key = "Sound",
                        Value = xSound,
                    },
                    new XValuePair {
                        Key = "TTS",
                        Value = xTTS,
                    },
                    new XValuePair {
                        Key = "Rate",
                        Value = xRate.ToString(CultureInfo.InvariantCulture),
                    },
                    new XValuePair {
                        Key = "Volume",
                        Value = xVolume.ToString(CultureInfo.InvariantCulture),
                    },
                    new XValuePair {
                        Key = "Delay",
                        Value = xDelay.ToString(CultureInfo.InvariantCulture),
                    },
                    new XValuePair {
                        Key = "Category",
                        Value = xCategory,
                    },
                    new XValuePair {
                        Key = "Enabled",
                        Value = xEnabled.ToString(),
                    },
                    new XValuePair {
                        Key = "Executable",
                        Value = xExecutable,
                    },
                    new XValuePair {
                        Key = "Arguments",
                        Value = xArguments,
                    },
                };
                XElement element = enumerable.FirstOrDefault(e => e.Attribute("Key").Value == xKey);
                if (element == null) {
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "Event", xKey, keyPairList);
                }
                else {
                    element.SetAttributeValue("Key", xKey);

                    foreach (XValuePair kv in keyPairList) {
                        XElement childElement = element.Element(kv.Key);
                        if (childElement == null) {
                            childElement = new XElement(kv.Key);
                            element.Add(childElement);
                        }

                        childElement.SetValue(kv.Value);
                    }
                }
            }
        }

        private void SaveSettingsNode() {
            if (Constants.XSettings == null) {
                return;
            }

            IEnumerable<XElement> xElements = Constants.XSettings.Descendants().Elements("Setting");
            XElement[] enumerable = xElements as XElement[] ?? xElements.ToArray();
            foreach (var setting in Constants.Settings) {
                XElement element = enumerable.FirstOrDefault(e => e.Attribute("Key").Value == setting);
                var xKey = setting;
                if (Default[xKey] == null) {
                    continue;
                }

                if (element == null) {
                    var xValue = Default[xKey].ToString();
                    List<XValuePair> keyPairList = new List<XValuePair> {
                        new XValuePair {
                            Key = "Value",
                            Value = xValue,
                        },
                    };
                    XmlHelper.SaveXmlNode(Constants.XSettings, "Settings", "Setting", xKey, keyPairList);
                }
                else {
                    XElement xElement = element.Element("Value");
                    if (xElement != null) {
                        xElement.Value = Default[setting].ToString();
                    }
                }
            }
        }
    }
}