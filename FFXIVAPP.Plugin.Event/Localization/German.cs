// FFXIVAPP.Plugin.Event ~ German.cs
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

using System.Windows;

namespace FFXIVAPP.Plugin.Event.Localization
{
    public abstract class German
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
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
