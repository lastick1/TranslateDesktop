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
        string[] targetLangKeys = { "en", "ru" };
        string[] sourceLangKeys = { "ru", "en" };
        string[] sourceTextArray = { "Привет", "Hi" };
        string[] expectTextArray = { "Hi", "Привет" };

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
            for (int i = 0; i < sourceLangKeys.Length; i++)
            {
                var source = sourceTextArray[i];
                var expect = sourceLangKeys[i];
                var actual = model.DetectLang(source).Key;
                Assert.AreEqual(expect, actual);
            }
        }

        [TestMethod]
        public void ModelTranslateTestMethod1()
        {
            for (int i = 0; i < targetLangKeys.Length; i++)
            {
                model.SetTargetLang(model.LangByKey(targetLangKeys[i]));
                // учитываем бизнес-логику
                // модель сама определяет язык исходного текста, если он не задан
                if (i > 0)
                    model.SetSourceLang(model.LangByKey(sourceLangKeys[i]));
                var target = targetLangKeys[i];
                var source = sourceTextArray[i];
                var expect = expectTextArray[i];
                var dir = model.TranslateDirection;
                var actual = model.Translate(source);
                dir = model.TranslateDirection;
                Assert.AreEqual(expect, actual,
                    string.Format("Model.Translate() works. Context #{2}: {0} for {1}",
                    dir,
                    source,
                    i
                    ));
            }
        }

        [TestMethod]
        public void ModelTranslateTestMethod2()
        {
            for (int i = targetLangKeys.Length-1; i >= 0 ; i--)
            {
                model.SetTargetLang(model.LangByKey(targetLangKeys[i]));
                // учитываем бизнес-логику
                // модель сама определяет язык исходного текста, если он не задан
                if (i < targetLangKeys.Length - 1)
                    model.SetSourceLang(model.LangByKey(sourceLangKeys[i]));
                var target = targetLangKeys[i];
                var source = sourceTextArray[i];
                var expect = expectTextArray[i];
                var dir = model.TranslateDirection;
                var actual = model.Translate(source);
                dir = model.TranslateDirection;
                Assert.AreEqual(expect, actual,
                    string.Format("Model.Translate() works. Context #{2}: {0} for {1}",
                    dir,
                    source,
                    i
                    ));
            }
        }
    }
}
