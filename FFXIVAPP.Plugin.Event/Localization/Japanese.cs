// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Japanese.cs" company="SyndicatedLife">
//   Copyright(c) 2018 Ryan Wilson &amp;lt;syndicated.life@gmail.com&amp;gt; (http://syndicated.life/)
//   Licensed under the MIT license. See LICENSE.md in the solution root for full license information.
// </copyright>
// <summary>
//   Japanese.cs Implementation
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FFXIVAPP.Plugin.Event.Localization {
    using System.Windows;

    public abstract class Japanese {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context() {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "イベントの追加および更新");
            Dictionary.Add("event_RegExHeader", "正規表現");
            Dictionary.Add("event_RegExLabel", "正規表現：");
            Dictionary.Add("event_SampleText", "スカウトヴァルチャーは「ウィングカッター」の構え。");
            Dictionary.Add("event_SoundHeader", "サウンド");
            Dictionary.Add("event_SoundLabel", "サウンド：");
            Dictionary.Add("event_DelayHeader", "遅延（秒）");
            Dictionary.Add("event_DelayLabel", "遅延（秒）：");
            Dictionary.Add("event_EnabledHeader", "有効");
            Dictionary.Add("event_CategoryLabel", "カテゴリ：");
            Dictionary.Add("event_CategoryHeader", "カテゴリ：");
            Dictionary.Add("event_MiscellaneousLabel", "一般");
            Dictionary.Add("event_RefreshSoundListButtonText", "サウンドリストを更新");
            Dictionary.Add("event_ExecutableLabel", "実行:");
            Dictionary.Add("event_SelectExecutableButtonText", "サウンドを選択");
            Dictionary.Add("event_ExecutableHeader", "実行：");
            Dictionary.Add("event_VolumeHeader", "ボリューム");
            Dictionary.Add("event_VolumeLabel", "ボリューム：");
            Dictionary.Add("event_TestSoundButtonText", "テスト");
            Dictionary.Add("event_GitHubButtonText", "プロジェクトのソースを開く（GitHub)");
            Dictionary.Add("event_EventOptionsHeader", "イベントオプション");
            Dictionary.Add("event_SoundOptionsHeader", "サウンドオプション");
            Dictionary.Add("event_ExecutableOptionsHeader", "実行オプション");
            Dictionary.Add("event_GlobalVolumeHeader", "全体ボリューム");
            Dictionary.Add("event_TTSHeader", "TTS");
            Dictionary.Add("event_RateHeader", "レート");
            Dictionary.Add("event_TTSLabel", "TTS");
            Dictionary.Add("event_RateLabel", "レート");
            Dictionary.Add("event_ArgumentsLabel", "引数");
            Dictionary.Add("event_FrendlyName", "イベント");
            return Dictionary;
        }
    }
}