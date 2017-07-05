using System;
using System.IO;
using TranslateDesktop;
using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using NSubstitute;

namespace UnitTest
{
    [TestFixture]
    public class TranslatorModelTests
    {
        [OneTimeSetUp]
        public void CwdSetup()
        {
            Directory.SetCurrentDirectory(TestContext.CurrentContext.WorkDirectory);
        }

        [Category("ValidData")]
        [TestCase("TestDataLangs.xml")]
        public void LangsProperty_ValidXmlResponse_ContainsSomething(string testDataFile)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();
            string responseLangs = File.ReadAllText(testDataFile);

            fakeYandexAPI.GetLangs(Arg.Any<string>(), Arg.Any<string>()).Returns(responseLangs);

            ITranslatorModel model = new TranslatorModel("api_key", "ui", fakeYandexAPI);

            var langs = model.Langs;

            Assert.IsTrue(model.Langs.Count > 0);
        }

        [Category("Throws")]
        [TestCase(null, "ru")]
        [TestCase("", "ru")]
        public void ctor_ApiKeyIsNullOrEmpty_Throws(string apiKey, string ui)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();

            Assert.Throws<TranslatorModelException>(() => new TranslatorModel(apiKey, ui, fakeYandexAPI));
        }

        [Category("Throws")]
        [TestCase("key", null)]
        [TestCase("key", "")]
        public void ctor_UiIsNullOrEmpty_Throws(string apiKey, string ui)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();

            Assert.Throws<TranslatorModelException>(() => new TranslatorModel(apiKey, ui, fakeYandexAPI));
        }

        [Category("Throws")]
        [TestCase("TestDataLangs_error.xml")]
        public void LangsProperty_ErrorXmlResponse_ThrowsExWithErrorMessage(string testDataFile)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();
            XmlDocument doc = new XmlDocument();
            string responseLangs = File.ReadAllText(testDataFile);
            doc.LoadXml(responseLangs);

            fakeYandexAPI.GetLangs(Arg.Any<string>(), Arg.Any<string>()).Returns(responseLangs);

            ITranslatorModel model = new TranslatorModel("api_key", "ui", fakeYandexAPI);

            Exception ex = Assert.Catch<Exception>(() => model.Langs.ToString());
            string expectedMessage = doc.LastChild.Attributes["message"].Value;

            StringAssert.Contains(expectedMessage, ex.Message);
        }

        [Category("ValidData")]
        [TestCase("TestDataLangs.xml", "en", "английский")] // верно ли задан атрибут?
        public void LangByKey_ValidXmlResponse_WorksCorrectly(string testDataFile, string key, string expect)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();
            string responseLangs = File.ReadAllText(testDataFile);

            fakeYandexAPI.GetLangs(Arg.Any<string>(), Arg.Any<string>()).Returns(responseLangs);

            ITranslatorModel model = new TranslatorModel("api_key", "ui", fakeYandexAPI);

            var l = model.LangByKey(key);

            Assert.AreEqual(l.Value, expect);
        }

        [Category("ValidData")]
        [TestCase("Hello", "TestDataDetect_en.xml", "TestDataLangs.xml", "en")]
        [TestCase("Привет", "TestDataDetect_ru.xml", "TestDataLangs.xml", "ru")]
        public void DetectLang_ValidXmlResponse_WorksCorrectly(
            string text,
            string testDataFile,
            string testDataLangsFile,
            string expect)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();
            string responseLangs = File.ReadAllText(testDataLangsFile);
            string responseDetect = File.ReadAllText(testDataFile);

            fakeYandexAPI.GetLangs(Arg.Any<string>(), Arg.Any<string>()).Returns(responseLangs);
            fakeYandexAPI.Detect(Arg.Any<string>(), text).Returns(responseDetect);

            ITranslatorModel model = new TranslatorModel("api_key", "ui", fakeYandexAPI);

            string actual = model.DetectLang(text).Key;

            Assert.AreEqual(expect, actual);
        }

        [Category("ValidData")]
        [TestCase("Hello World!", "en-ru", "TestDataTranslate_en-ru.xml", "TestDataDetect_en.xml", "TestDataLangs.xml", "Здравствуй, Мир!")]
        [TestCase("Привет", "ru-en", "TestDataTranslate_ru-en.xml", "TestDataDetect_ru.xml", "TestDataLangs.xml", "Hi")]
        public void Translate_ValidXmlResponse_WorksCorrectly(
            string text,
            string sourceLang,
            string testDataFile,
            string testDetectDataFile,
            string testDataLangsFile,
            string expect)
        {
            ITranslateAPI fakeYandexAPI = Substitute.For<ITranslateAPI>();
            string responseLangs = File.ReadAllText(testDataLangsFile);
            string responseTranslate = File.ReadAllText(testDataFile);
            string responseDetect = File.ReadAllText(testDetectDataFile);
            
            fakeYandexAPI.GetLangs(Arg.Any<string>(), Arg.Any<string>()).Returns(responseLangs);
            fakeYandexAPI.Detect(Arg.Any<string>(), text).Returns(responseDetect);
            fakeYandexAPI.Translate(Arg.Any<string>(), text, sourceLang).Returns(responseTranslate);

            ITranslatorModel model = new TranslatorModel("api_key", "ui", fakeYandexAPI);

            model.SetTargetLang(model.LangByKey(sourceLang.Split('-')[1]));
            string actual = model.Translate(text);

            Assert.AreEqual(expect, actual, "Model.Translate() works.");
        }
    }
}
