using System;
using System.Collections.Generic;
using TranslateDesktop;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class ViewModelTest
    {
        [TestMethod]
        public void ViewModelTranslateCommandTestMethod()
        {
            TranslatorViewModel vm = new TranslatorViewModel();
            vm.SrcText = "hi";
            // первое срабатывание команды допускает, чтобы пользователь не выбирал язык исходного текста
            vm.SelectedDestLang = new KeyValuePair<string, string>("ru", "РусТест");
            vm.TranslateCommand.Execute(null);
            Assert.AreEqual("привет", vm.DestText, 
                string.Format("TranslatorViewModel works 1. Context: {0}-{1}:{2}-{3}",
                vm.SelectedSrcLang,
                vm.SelectedDestLang,
                vm.SrcText,
                vm.DestText));


            vm.SrcText = "привет";
            // следующей строчкой имитируется выбор пользователем другого языка исходного текста
            vm.SelectedSrcLang = new KeyValuePair<string, string>("ru", "РусТест");
            vm.SelectedDestLang = new KeyValuePair<string, string>("en", "АнглТест");
            vm.TranslateCommand.Execute(null);
            Assert.AreEqual("hi", vm.DestText, string.Format("TranslatorViewModel works 2. Context: {0}-{1}:{2}-{3}",
                vm.SelectedSrcLang,
                vm.SelectedDestLang,
                vm.SrcText,
                vm.DestText));
        }
    }
}
