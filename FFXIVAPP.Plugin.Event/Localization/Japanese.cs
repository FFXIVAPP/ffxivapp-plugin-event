// FFXIVAPP.Plugin.Event ~ Japanese.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
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
    public abstract class Japanese
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "イベントの追加/保存");
            Dictionary.Add("event_RegExHeader", "正規表現");
            Dictionary.Add("event_RegExLabel", "正規表現:");
            Dictionary.Add("event_SampleText", "スカウトヴァルチャーは「ウィングカッター」の構え。");
            Dictionary.Add("event_SoundHeader", "サウンド");
            Dictionary.Add("event_SoundLabel", "サウンド:");
            Dictionary.Add("event_DelayHeader", "遅延(秒)");
            Dictionary.Add("event_DelayLabel", "遅延(秒):");
            Dictionary.Add("event_EnabledHeader", "有効");
            Dictionary.Add("event_CategoryLabel", "カテゴリ:");
            Dictionary.Add("event_CategoryHeader", "カテゴリ:");
            Dictionary.Add("event_MiscellaneousLabel", "一般");
            Dictionary.Add("event_RefreshSoundListButtonText", "サウンドリストを更新");
            Dictionary.Add("event_ExecutableLabel", "実行:");
            Dictionary.Add("event_SelectExecutableButtonText", "音を選択");
            Dictionary.Add("event_ExecutableHeader", "実行:");
            Dictionary.Add("event_VolumeHeader", "音量");
            Dictionary.Add("event_VolumeLabel", "音量:");
            Dictionary.Add("event_TestSoundButtonText", "テスト");
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
