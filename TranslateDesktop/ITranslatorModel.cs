using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateDesktop
{
    public interface ITranslatorModel
    {
        /// <summary>
        /// API-ключ
        /// </summary>
        string ApiKey { get; }

        /// <summary>
        /// Язык пользовательского интерфейся
        /// </summary>
        string UiLang { get; }

        /// <summary>
        /// Задать целевой язык для перевода
        /// </summary>
        void SetTargetLang(KeyValuePair<string, string> target);

        /// <summary>
        /// Задать язык исходного текста
        /// </summary>
        void SetSourceLang(KeyValuePair<string, string> source);

        /// <summary>
        /// Выбранный язык исходного текста
        /// </summary>
        KeyValuePair<string, string> SourceLang { get; }

        /// <summary>
        /// Направление перевода
        /// </summary>
        string TranslateDirection { get; }

        /// <summary>
        /// Получение списка языков
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> Langs { get; }

        /// <summary>
        /// Язык по его коду
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        KeyValuePair<string, string> LangByKey(string key);

        /// <summary>
        /// Определение языка текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        KeyValuePair<string, string> DetectLang(string text);

        /// <summary>
        /// Перевод текста на язык
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        string Translate(string text);
    }
}
