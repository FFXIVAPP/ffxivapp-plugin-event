// FFXIVAPP.Plugin.Event
// Japanese.cs
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
            return Dictionary;
        }
    }
}
