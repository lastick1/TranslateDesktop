using System.IO;
using System.Configuration;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Xml;
using Moq;

namespace UnitTest
{
    [TestClass]
    public class ModelTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        [DeploymentItem("TestDataLangs.xml")]
        public void ModelLangsPropertyTestMethod()
        {
            string responseLangs = File.ReadAllText("TestDataLangs.xml");
            string apiKey = "test_key";
            string ui = "test_ui";
            var mock = new Mock<ITranslateAPI>();

            mock.Setup(tm => tm.GetLangs(apiKey, ui)).Returns(responseLangs);

            ITranslatorModel model = new TranslatorModel(apiKey, ui, mock.Object);

            var langs = model.Langs;

            Assert.IsTrue(model.Langs.Count > 0, "Model.Langs contains something");
        }

        [TestMethod]
        [DeploymentItem("TestDataLangs.xml")]
        public void ModelLangByKeyTestMethod()
        {
            string responseLangs = File.ReadAllText("TestDataLangs.xml");
            string apiKey = "test_key";
            string ui = "test_ui";
            var mock = new Mock<ITranslateAPI>();

            mock.Setup(tm => tm.GetLangs(apiKey, ui)).Returns(responseLangs);

            ITranslatorModel model = new TranslatorModel(apiKey, ui, mock.Object);

            var l = model.LangByKey("en");

            Assert.AreEqual(l.Value, "английский");
        }

        [TestMethod]
        [DeploymentItem("TestCasesDetect.xml")]
        [DeploymentItem("TestDataLangs.xml")]
        [DeploymentItem("TestDataDetect_en.xml")]
        [DeploymentItem("TestDataDetect_ru.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   "|DataDirectory|\\TestCasesDetect.xml",
                   "TestCase",
                    DataAccessMethod.Sequential)]
        public void ModelDetectLangTestMethod()
        {
            string responseLangs = File.ReadAllText("TestDataLangs.xml");
            string responseDetect = File.ReadAllText((string)TestContext.DataRow["Response"]);
            string apiKey = "test_key";
            string ui = "test_ui";
            var source = (string)TestContext.DataRow["Text"]; 
            var expect = (string)TestContext.DataRow["Expect"];
            var mock = new Mock<ITranslateAPI>();

            mock.Setup(tm => tm.GetLangs(apiKey, ui)).Returns(responseLangs);
            mock.Setup(tm => tm.Detect(apiKey, source)).Returns(responseDetect);

            ITranslatorModel model = new TranslatorModel(apiKey, ui, mock.Object);

            var actual = model.DetectLang(source).Key;

            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        [DeploymentItem("TestCasesTranslate.xml")]
        [DeploymentItem("TestDataLangs.xml")]
        [DeploymentItem("TestDataTranslate_ru-en.xml")]
        [DeploymentItem("TestDataTranslate_en-ru.xml")]
        [DeploymentItem("TestDataDetect_en.xml")]
        [DeploymentItem("TestDataDetect_ru.xml")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                   "|DataDirectory|\\TestCasesTranslate.xml",
                   "TestCase",
                    DataAccessMethod.Random)]
        public void ModelTranslateTestMethod()
        {
            string responseLangs = File.ReadAllText("TestDataLangs.xml");
            string responseTranslate = File.ReadAllText((string)TestContext.DataRow["Response"]);
            string responseDetect = File.ReadAllText((string)TestContext.DataRow["Detect"]);
            string apiKey = "test_key";
            string ui = "test_ui";
            var source = (string)TestContext.DataRow["Text"];
            var lang = (string)TestContext.DataRow["Lang"];
            var expect = (string)TestContext.DataRow["Expect"];
            var mock = new Mock<ITranslateAPI>();

            mock.Setup(tm => tm.GetLangs(apiKey, ui)).Returns(responseLangs);
            mock.Setup(tm => tm.Detect(apiKey, source)).Returns(responseDetect);
            mock.Setup(tm => tm.Translate(apiKey, source, lang)).Returns(responseTranslate);

            ITranslatorModel model = new TranslatorModel(apiKey, ui, mock.Object);

            model.SetTargetLang(model.LangByKey(lang.Split('-')[1]));
            var actual = model.Translate(source);

            Assert.AreEqual(expect, actual, "Model.Translate() works.");
        }
    }
}
