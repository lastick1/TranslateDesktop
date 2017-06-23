using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Xml;
using System.IO;

namespace TranslateDesktop
{
    public class YandexTranslateAPI
    {
        private static Uri translateUri = new Uri(@"https://translate.yandex.net/api/v1.5/tr/translate");
        private static Uri getLangsUri = new Uri(@"https://translate.yandex.net/api/v1.5/tr/getLangs");
        private static Uri detectUri = new Uri(@"https://translate.yandex.net/api/v1.5/tr/detect");

        /// <summary>
        /// Метод для получения списка доступных языков (формат ответа XML)
        /// </summary>
        /// <param name="apiKey">API-ключ</param>
        /// <param name="ui">язык интерфейса</param>
        /// <returns></returns>
        public static string GetLangs(string apiKey, string ui)
        {
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(
                    getLangsUri, 
                    new NameValueCollection()
                        {
                            { "key", apiKey }, { "ui", ui }
                        }
                    );
                return Encoding.UTF8.GetString(response);
            }
        }

        /// <summary>
        /// Метод для определения языка (формат ответа XML)
        /// </summary>
        /// <param name="apiKey">API-ключ</param>
        /// <param name="text">текст, язык которого требуется определить</param>
        /// <returns></returns>
        public static string Detect(string apiKey, string text)
        {
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(
                    detectUri,
                    new NameValueCollection()
                        {
                            { "key", apiKey }, { "text", text }
                        }
                    );
                return Encoding.UTF8.GetString(response);
            }
        }

        /// <summary>
        /// Перевод текста на заданный язык (формат ответа XML)
        /// </summary>
        /// <param name="apiKey">API-ключ</param>
        /// <param name="text">текст, который требуется перевести</param>
        /// <param name="lang">направление перевода [код_языка_текста-]код_языка_перевода</param>
        /// <returns></returns>
        public static string Translate(string apiKey, string text, string lang)
        {
            using (WebClient client = new WebClient())
            {
                byte[] response = client.UploadValues(
                    translateUri,
                    new NameValueCollection()
                        {
                            { "key", apiKey }, { "text", text }, { "lang", lang }
                        }
                    );
                return Encoding.UTF8.GetString(response);
            }
        }
    }
}
