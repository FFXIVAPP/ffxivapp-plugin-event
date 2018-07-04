// --------------------------------------------------------------------------------------------------------------------
// <copyright file="German.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   German.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Localization {
    using System.Windows;

    public abstract class German {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "Ereignis hinzufügen oder updaten");
            Dictionary.Add("event_RegExHeader", "RegEx");
            Dictionary.Add("event_RegExLabel", "RegEx:");
            Dictionary.Add("event_SampleText", "The scout vulture readies Wing Cutter.");
            Dictionary.Add("event_SoundHeader", "Klang");
            Dictionary.Add("event_SoundLabel", "Klang:");
            Dictionary.Add("event_DelayHeader", "Verzögerung (secs)");
            Dictionary.Add("event_DelayLabel", "Verzögerung (secs):");
            Dictionary.Add("event_EnabledHeader", "Enabled");
            Dictionary.Add("event_CategoryLabel", "Category:");
            Dictionary.Add("event_CategoryHeader", "Category");
            Dictionary.Add("event_MiscellaneousLabel", "Miscellaneous");
            Dictionary.Add("event_RefreshSoundListButtonText", "Refresh Sound List");
            Dictionary.Add("event_ExecutableLabel", "Run:");
            Dictionary.Add("event_SelectExecutableButtonText", "Select Executable");
            Dictionary.Add("event_ExecutableHeader", "Run");
            Dictionary.Add("event_VolumeHeader", "Volume");
            Dictionary.Add("event_VolumeLabel", "Volume:");
            Dictionary.Add("event_TestSoundButtonText", "Test");
            Dictionary.Add("event_GitHubButtonText", "Open Project Source (GitHub)");
            Dictionary.Add("event_EventOptionsHeader", "Event Options");
            Dictionary.Add("event_SoundOptionsHeader", "Sound Options");
            Dictionary.Add("event_ExecutableOptionsHeader", "Executable Options");
            Dictionary.Add("event_GlobalVolumeHeader", "Global Volume");
            Dictionary.Add("event_TTSHeader", "TTS");
            Dictionary.Add("event_RateHeader", "Rate");
            Dictionary.Add("event_TTSLabel", "TTS");
            Dictionary.Add("event_RateLabel", "Rate");
            Dictionary.Add("event_ArgumentsLabel", "Arguments");
            return Dictionary;
        }
    }
}