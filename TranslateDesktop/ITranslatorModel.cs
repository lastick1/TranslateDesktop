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
        /// Целевой язык для перевода
        /// </summary>
        string TargetLang { get; set; }

        /// <summary>
        /// Асинхронное получение списка языков
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> Langs { get; }

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
