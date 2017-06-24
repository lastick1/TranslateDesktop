using System;
using System.Configuration;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class ModelTest
    {
        TranslatorModel model = new TranslatorModel();

        [TestMethod]
        public void ModelLangsPropertyTestMethod()
        {
            Assert.IsInstanceOfType(model.Langs, typeof(Dictionary<string, string>), "Model.Langs is dictionary");
            Assert.IsTrue(model.Langs.Count > 0, "Model.Langs contains something");
        }

        [TestMethod]
        public void ModelLangByKeyTestMethod()
        {
            var l = model.LangByKey("en");
            Assert.AreEqual(l.Value, "Английский");
        }

        [TestMethod]
        public void ModelDetectLangTestMethod()
        {
            var d = model.DetectLang("Пока");
            Assert.IsTrue(model.Langs.ContainsKey(d.Key), "Model.DetectLang() works");
            Assert.IsTrue(model.Langs.ContainsValue(d.Value), "Model.DetectLang() works");
        }

        [TestMethod]
        public void ModelTranslateTestMethod()
        {
            model.TargetLang = "en";
            Assert.AreEqual(model.Translate("привет"), "hi", "Model.Translate() works");
        }
    }
}
