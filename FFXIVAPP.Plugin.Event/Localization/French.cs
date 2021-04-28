// --------------------------------------------------------------------------------------------------------------------
// <copyright file="French.cs" company="SyndicatedLife">
//   Copyright© 2007 - 2021 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (https://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   French.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Localization {
    using System.Windows;

    public abstract class French {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "Ajouter ou mettre à jour un évenement");
            Dictionary.Add("event_RegExHeader", "RegEx");
            Dictionary.Add("event_RegExLabel", "RegEx:");
            Dictionary.Add("event_SampleText", "The scout vulture readies Wing Cutter.");
            Dictionary.Add("event_SoundHeader", "Son");
            Dictionary.Add("event_SoundLabel", "Son:");
            Dictionary.Add("event_DelayHeader", "Délai (secs)");
            Dictionary.Add("event_DelayLabel", "Délai (secs):");
            Dictionary.Add("event_EnabledHeader", "Activé");
            Dictionary.Add("event_CategoryLabel", "Categorie:");
            Dictionary.Add("event_CategoryHeader", "Categorie");
            Dictionary.Add("event_MiscellaneousLabel", "Divers");
            Dictionary.Add("event_RefreshSoundListButtonText", "Rafraîchir la liste des sons");
            Dictionary.Add("event_ExecutableLabel", "Exécuter:");
            Dictionary.Add("event_SelectExecutableButtonText", "Sélectionner un exécutable");
            Dictionary.Add("event_ExecutableHeader", "Exécuter");
            Dictionary.Add("event_VolumeHeader", "Volume");
            Dictionary.Add("event_VolumeLabel", "Volume:");
            Dictionary.Add("event_TestSoundButtonText", "Test");
            Dictionary.Add("event_GitHubButtonText", "Open Project Source (GitHub)");
            Dictionary.Add("event_EventOptionsHeader", "Event Options");
            Dictionary.Add("event_SoundOptionsHeader", "Sound Options");
            Dictionary.Add("event_ExecutableOptionsHeader", "Executable Options");
            Dictionary.Add("event_GlobalVolumeHeader", "Volume global");
            Dictionary.Add("event_TTSHeader", "TTS");
            Dictionary.Add("event_RateHeader", "Rate");
            Dictionary.Add("event_TTSLabel", "TTS");
            Dictionary.Add("event_RateLabel", "Rate");
            Dictionary.Add("event_ArgumentsLabel", "Arguments");
            return Dictionary;
        }
    }
}