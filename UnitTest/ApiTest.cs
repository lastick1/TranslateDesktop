using System;
using System.Configuration;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        private static string key = ConfigurationManager.AppSettings["apiKey"];
        private static string ui = ConfigurationManager.AppSettings["ui"];
        [TestMethod]
        public void ApiGetLangsTestMethod()
        {

            string val = YandexTranslateAPI.GetLangs(key, ui);
            Assert.IsFalse(string.IsNullOrEmpty(val), "GetLangs() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "GetLangs() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("</Langs>"), "GetLangs() value ends with </Langs> : " + val);
        }

        [TestMethod]
        public void ApiDetectTestMethod()
        {
            string val = YandexTranslateAPI.Detect(key, "привет");
            Assert.IsFalse(string.IsNullOrEmpty(val), "Detect() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Detect() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("/>"), "Detect() value ends with /> : " + val);
        }

        [TestMethod]
        public void ApiTranslateTestMethod()
        {
            string val = YandexTranslateAPI.Translate(key, "привет", "en");
            Assert.IsFalse(string.IsNullOrEmpty(val), "Translate() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Translate() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("</Translation>"), "Translate() value ends with </Translation> : " + val);
        }
    }
}
