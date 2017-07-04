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
    /// <summary>
    /// Модель переводчика.
    /// </summary>
    public class TranslatorModel : ITranslatorModel
    {
        #region Поля
        private Dictionary<string, string> _langs = null;
        private string srcLangKey;
        private string targetLangKey;
        private ITranslateAPI _api;
        #endregion

        #region Свойства
        public string ApiKey { get; }

        public string UiLang { get; }

        public KeyValuePair<string, string> SourceLang
        {
            get { return LangByKey(srcLangKey); }
        }

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
        
        public string TranslateDirection
        {
            get
            {
                if (string.IsNullOrEmpty(srcLangKey))
                {
                    return targetLangKey;
                }
                return string.Format("{0}-{1}", srcLangKey, targetLangKey);
            }
        }
        #endregion

        public TranslatorModel(string apiKey, string uiLang, ITranslateAPI api)
        {
            _api = api;
            ApiKey = apiKey;
            UiLang = uiLang;
        }

        #region Методы
        public void SetTargetLang(KeyValuePair<string, string> lang)
        {
            targetLangKey = lang.Key;
        }
        public void SetSourceLang(KeyValuePair<string, string> lang)
        {
            srcLangKey = lang.Key;
        }

        public KeyValuePair<string, string> DetectLang(string text)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(_api.Detect(ApiKey, text));
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
            if (string.IsNullOrEmpty(srcLangKey))
            {
                SetSourceLang(DetectLang(text));
            }
            XmlDocument document = new XmlDocument();
            var xml = _api.Translate(ApiKey, text, TranslateDirection);
            document.LoadXml(xml);
            foreach(XmlElement e in document.GetElementsByTagName("text"))
            {
                return e.InnerText;
            }
            return "";
        }
        #endregion

        #region Private Методы
        private Dictionary<string, string> GetLangs()
        {
            Dictionary<string, string> langs = new Dictionary<string, string>();
            XmlDocument document = new XmlDocument();
            var xml = _api.GetLangs(ApiKey, UiLang);
            document.LoadXml(xml);
            foreach (XmlElement e in document.GetElementsByTagName("Item"))
            {
                langs.Add(e.Attributes["key"].Value, e.Attributes["value"].Value);
            }
            return langs;
        }
        #endregion
    }
}
