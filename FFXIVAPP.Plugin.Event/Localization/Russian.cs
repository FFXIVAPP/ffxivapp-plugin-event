// FFXIVAPP.Plugin.Event
// Russian.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// Modified by Yaguar666 ak Yaguar Kuro
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
    public abstract class Russian
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_", "*PH*");
            Dictionary.Add("event_HeaderTabName", "События");
            Dictionary.Add("event_AddUpdateEventButtonText", "Добавить или обновить Событие");
            Dictionary.Add("event_RegExHeader", "Событие в чате");
            Dictionary.Add("event_RegExLabel", "Событие в чате:");
            Dictionary.Add("event_SampleText", "Scout vulture reading Wing Cutter.");
            Dictionary.Add("event_SoundHeader", "Звук");
            Dictionary.Add("event_SoundLabel", "Звук:");
            Dictionary.Add("event_DelayHeader", "Задержка (секунды)");
            Dictionary.Add("event_DelayLabel", "Задержка (секунды):");
            Dictionary.Add("event_EnabledHeader", "Активирован");
            Dictionary.Add("event_CategoryLabel", "Категория:");
            Dictionary.Add("event_CategoryHeader", "Категория");
            Dictionary.Add("event_MiscellaneousLabel", "Разное");
            Dictionary.Add("event_RefreshSoundListButtonText", "Обновить Лист Звуков");
            Dictionary.Add("event_ExecutableLabel", "Запуск:");
            Dictionary.Add("event_SelectExecutableButtonText", "Выбрать Выполняемые");
            Dictionary.Add("event_ExecutableHeader", "Запуск");
            Dictionary.Add("event_VolumeHeader", "Громкость");
            Dictionary.Add("event_VolumeLabel", "Гроскость:");
            Dictionary.Add("event_TestSoundButtonText", "Тест");
            Dictionary.Add("event_GitHubButtonText", "Страница проекта (GitHub)");
            Dictionary.Add("event_EventOptionsHeader", "Опции События");
            Dictionary.Add("event_SoundOptionsHeader", "Опции Звука");
            Dictionary.Add("event_ExecutableOptionsHeader", "Выполняемые Опции");
            Dictionary.Add("event_GlobalVolumeHeader", "Глобальная Громкость");
            Dictionary.Add("event_TTSHeader", "Произносимый Текст");
            Dictionary.Add("event_RateHeader", "Скорость Чтения");
            Dictionary.Add("event_TTSLabel", "Произносимый Текст");
            Dictionary.Add("event_RateLabel", "Скорость Чтения");
            Dictionary.Add("event_ArgumentsLabel", "Аргументы");
            return Dictionary;
        }
    }
}
