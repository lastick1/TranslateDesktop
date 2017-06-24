using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TranslateDesktop
{
    public class TranslatorModel : ITranslatorModel
    {
        #region Свойства
        public string ApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["apiKey"];
            }
        }

        public string UiLang
        {
            get
            {
                return ConfigurationManager.AppSettings["ui"];
            }
        }

        public string TargetLang { get; set; }

        private Dictionary<string, string> _langs = null;
        public Dictionary<string, string> Langs
        {
            get
            {
                if (_langs == null)
                {
                    _langs = GetLangs();
                }
                return _langs;
            }
        }
        #endregion

        public KeyValuePair<string, string> DetectLang(string text)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(YandexTranslateAPI.Detect(ApiKey, text));
            foreach(XmlElement e in document.GetElementsByTagName("DetectedLang"))
            {
                return LangByKey(e.Attributes["lang"].Value);
            }
            return new KeyValuePair<string, string>();
        }
        public KeyValuePair<string, string> LangByKey(string key)
        {
            return Langs.First((x) => x.Key == key);
        }

        public string Translate(string text)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(YandexTranslateAPI.Translate(ApiKey, text, TargetLang));
            foreach(XmlElement e in document.GetElementsByTagName("text"))
            {
                return e.InnerText;
            }
            return "";
        }

        #region Private Методы
        private Dictionary<string, string> GetLangs()
        {
            Dictionary<string, string> langs = new Dictionary<string, string>();
            XmlDocument document = new XmlDocument();
            document.LoadXml(YandexTranslateAPI.GetLangs(ApiKey, UiLang));
            foreach (XmlElement e in document.GetElementsByTagName("Item"))
            {
                langs.Add(e.Attributes["key"].Value, e.Attributes["value"].Value);
            }
            return langs;
        }
        #endregion


    }
}
