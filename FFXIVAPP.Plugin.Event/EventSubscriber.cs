// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSubscriber.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   EventSubscriber.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event {
    using System;

    using FFXIVAPP.Common.Core.Constant;
    using FFXIVAPP.Common.Models;
    using FFXIVAPP.Common.Utilities;
    using FFXIVAPP.IPluginInterface.Events;
    using FFXIVAPP.Plugin.Event.Utilities;

    using NLog;

    using Sharlayan.Core;

    public static class EventSubscriber {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Subscribe() {
            Plugin.PHost.ConstantsUpdated += OnConstantsUpdated;
            Plugin.PHost.ChatLogItemReceived += OnChatLogItemReceived;
        }

        public static void UnSubscribe() {
            Plugin.PHost.ConstantsUpdated -= OnConstantsUpdated;
            Plugin.PHost.ChatLogItemReceived -= OnChatLogItemReceived;
        }

        private static void OnChatLogItemReceived(object sender, ChatLogItemEvent chatLogItemEvent) {
            // delegate event from chat log, not required to subsribe
            // this updates 100 times a second and only sends a line when it gets a new one
            if (sender == null) {
                return;
            }

            ChatLogItem chatLogEntry = chatLogItemEvent.ChatLogItem;
            try {
                LogPublisher.Process(chatLogEntry);
            }
            catch (Exception ex) {
                Logging.Log(Logger, new LogItem(ex, true));
            }
        }

        private static void OnConstantsUpdated(object sender, ConstantsEntityEvent constantsEntityEvent) {
            // delegate event from constants, not required to subsribe, but recommended as it gives you app settings
            if (sender == null) {
                return;
            }

            ConstantsEntity constantsEntity = constantsEntityEvent.ConstantsEntity;
            Constants.AutoTranslate = constantsEntity.AutoTranslate;
            Constants.ChatCodes = constantsEntity.ChatCodes;
            Constants.Colors = constantsEntity.Colors;
            Constants.CultureInfo = constantsEntity.CultureInfo;
            Constants.CharacterName = constantsEntity.CharacterName;
            Constants.ServerName = constantsEntity.ServerName;
            Constants.GameLanguage = constantsEntity.GameLanguage;
            Constants.EnableHelpLabels = constantsEntity.EnableHelpLabels;
            Constants.Theme = constantsEntity.Theme;
            PluginViewModel.Instance.EnableHelpLabels = Constants.EnableHelpLabels;
        }
    }
}