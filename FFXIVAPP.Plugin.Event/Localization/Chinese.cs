// FFXIVAPP.Plugin.Event
// Chinese.cs
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
    public abstract class Chinese
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "新增或改变事件");
            Dictionary.Add("event_RegExHeader", "RegEx");
            Dictionary.Add("event_RegExLabel", "RegEx:");
            Dictionary.Add("event_SampleText", "侦察兵秃鹫翼刀已经准备好.");
            Dictionary.Add("event_SoundHeader", "音效");
            Dictionary.Add("event_SoundLabel", "音效:");
            Dictionary.Add("event_DelayHeader", "延迟 (秒)");
            Dictionary.Add("event_DelayLabel", "延迟 (秒):");
            Dictionary.Add("event_EnabledHeader", "开启");
            Dictionary.Add("event_CategoryLabel", "类别:");
            Dictionary.Add("event_CategoryHeader", "类别");
            Dictionary.Add("event_MiscellaneousLabel", "杂项");
            Dictionary.Add("event_RefreshSoundListButtonText", "刷新音效列表");
            Dictionary.Add("event_ExecutableLabel", "执行:");
            Dictionary.Add("event_SelectExecutableButtonText", "选择执行模块");
            Dictionary.Add("event_ExecutableHeader", "执行");
            Dictionary.Add("event_VolumeHeader", "音量");
            Dictionary.Add("event_VolumeLabel", "音量:");
            Dictionary.Add("event_TestSoundButtonText", "测试");
            Dictionary.Add("event_GitHubButtonText", "打开项目源代码 (GitHub)");
            Dictionary.Add("event_EventOptionsHeader", "事件选项");
            Dictionary.Add("event_SoundOptionsHeader", "音效选项");
            Dictionary.Add("event_ExecutableOptionsHeader", "执行模块选项");
            Dictionary.Add("event_GlobalVolumeHeader", "全局音量");
            return Dictionary;
        }
    }
}
