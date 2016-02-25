// FFXIVAPP.Plugin.Event ~ Russian.cs
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
