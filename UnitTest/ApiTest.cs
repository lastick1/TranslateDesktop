using System;
using System.Configuration;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        // надо убрать зависимость от соединения с интернет, 
        // т.е. использовать мок вместо httpWebRequest
        // пока не очень понятно как переделать на mock обращение к вебу
        // мок вернёт то же, что ему дать и получится тест 2 == 2, 
        // где логика? :)
        // возможно, переписать класс YandexTranslateAPI, 
        // чтобы ему в конструктор передавался интерфейс IHttpWebRequest 

        [TestMethod]
        public void ApiGetLangsTestMethod()
        {
            string key = ConfigurationManager.AppSettings["apiKey"];
            string ui = ConfigurationManager.AppSettings["ui"];
            var api = new YandexTranslateAPI();

            string actual = api.GetLangs(key, ui);

            Assert.IsFalse(string.IsNullOrEmpty(actual), "GetLangs() returns something");
            Assert.IsTrue(actual.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "GetLangs() value begins with XML Declaration : " + actual);
            Assert.IsTrue(actual.EndsWith("</Langs>"), "GetLangs() value ends with </Langs> : " + actual);
        }

        [TestMethod]
        public void ApiDetectTestMethod()
        {
            string key = ConfigurationManager.AppSettings["apiKey"];
            string ui = ConfigurationManager.AppSettings["ui"];
            var api = new YandexTranslateAPI();

            string actual = api.Detect(key, "привет");

            Assert.IsFalse(string.IsNullOrEmpty(actual), "Detect() returns something");
            Assert.IsTrue(actual.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Detect() value begins with XML Declaration : " + actual);
            Assert.IsTrue(actual.EndsWith("/>"), "Detect() value ends with /> : " + actual);
        }

        [TestMethod]
        public void ApiTranslateTestMethod()
        {
            string key = ConfigurationManager.AppSettings["apiKey"];
            string ui = ConfigurationManager.AppSettings["ui"];
            var api = new YandexTranslateAPI();

            string actual = api.Translate(key, "привет", "en");

            Assert.IsFalse(string.IsNullOrEmpty(actual), "Translate() returns something");
            Assert.IsTrue(actual.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Translate() value begins with XML Declaration : " + actual);
            Assert.IsTrue(actual.EndsWith("</Translation>"), "Translate() value ends with </Translation> : " + actual);
        }
    }
}
