using System;
using System.Configuration;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ApiTest
    {
        private static string key = ConfigurationManager.AppSettings["apiKey"];
        private static string ui = ConfigurationManager.AppSettings["ui"];
        [TestMethod]
        public void GetLangsTestMethod()
        {
            
            string val = YandexTranslateAPI.GetLangs(key, ui);
            Assert.IsFalse(string.IsNullOrEmpty(val), "GetLangs() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "GetLangs() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("</Langs>"), "GetLangs() value ends with </Langs> : " + val);
        }

        [TestMethod]
        public void DetectTestMethod()
        {
            string val = YandexTranslateAPI.Detect(key, "привет");
            Assert.IsFalse(string.IsNullOrEmpty(val), "Detect() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Detect() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("/>"), "Detect() value ends with /> : " + val);
        }

        [TestMethod]
        public void TranslateTestMethod()
        {
            string val = YandexTranslateAPI.Translate(key, "привет", "en");
            Assert.IsFalse(string.IsNullOrEmpty(val), "Translate() returns something");
            Assert.IsTrue(val.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"), "Translate() value begins with XML Declaration : " + val);
            Assert.IsTrue(val.EndsWith("</Translation>"), "Translate() value ends with </Translation> : " + val);
        }
    }
}
