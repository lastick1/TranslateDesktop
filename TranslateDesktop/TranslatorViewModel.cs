using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TranslateDesktop
{
    /// <summary>
    /// Модель представления переводчика по MVVM
    /// </summary>
    public class TranslatorViewModel: DependencyObject
    {
        public KeyValuePair<string, string> SelectedSrcLang
        {
            get { return (KeyValuePair<string, string>)GetValue(SelectedSrcLangProperty); }
            set
            {
                SetValue(SelectedSrcLangProperty, value);
                model.SetSourceLang(value);
            }
        }

        public static readonly DependencyProperty SelectedSrcLangProperty =
            DependencyProperty.Register("SelectedSrcLang", typeof(KeyValuePair<string, string>), typeof(TranslatorViewModel), new PropertyMetadata(null));



        public KeyValuePair<string, string> SelectedDestLang
        {
            get { return (KeyValuePair<string, string>)GetValue(SelectedDestLangProperty); }
            set
            {
                SetValue(SelectedDestLangProperty, value);
                model.SetTargetLang(value);
            }
        }

        public static readonly DependencyProperty SelectedDestLangProperty =
            DependencyProperty.Register("SelectedDestLang", typeof(KeyValuePair<string, string>), typeof(TranslatorViewModel), new PropertyMetadata(null));



        public Dictionary<string, string> SrcLangs
        {
            get { return (Dictionary<string, string>)GetValue(SrcLangsProperty); }
            set { SetValue(SrcLangsProperty, value); }
        }

        public static readonly DependencyProperty SrcLangsProperty =
            DependencyProperty.Register("SrcLangs", typeof(Dictionary<string, string>), typeof(TranslatorViewModel), new PropertyMetadata(null));



        public Dictionary<string, string> DestLangs
        {
            get { return (Dictionary<string, string>)GetValue(DestLangsProperty); }
            set { SetValue(DestLangsProperty, value); }
        }

        public static readonly DependencyProperty DestLangsProperty =
            DependencyProperty.Register("DestLangs", typeof(Dictionary<string, string>), typeof(TranslatorViewModel), new PropertyMetadata(null));



        public string SrcText
        {
            get { return (string)GetValue(SrcTextProperty); }
            set { SetValue(SrcTextProperty, value); }
        }

        public static readonly DependencyProperty SrcTextProperty =
            DependencyProperty.Register("SrcText", typeof(string), typeof(TranslatorViewModel), new PropertyMetadata(""));



        public string DestText
        {
            get { return (string)GetValue(DestTextProperty); }
            set { SetValue(DestTextProperty, value); }
        }

        public static readonly DependencyProperty DestTextProperty =
            DependencyProperty.Register("DestText", typeof(string), typeof(TranslatorViewModel), new PropertyMetadata(""));




        public Command TranslateCommand { get; }

        private ITranslatorModel model;

        public TranslatorViewModel()
        {
            model = new TranslatorModel(
                ConfigurationManager.AppSettings["apiKey"],
                ConfigurationManager.AppSettings["ui"],
                new YandexTranslateAPI());
            // SrcLangs реализовано в виде свойста зависимости специально,
            // чтобы оно поддерживало асинхронную загрузку,
            // если таковая будет реализована в дальнейшем
            SrcLangs = model.Langs;
            SelectedDestLang = model.Langs.First((x) => x.Key == "en");
            DestLangs = model.Langs;
            TranslateCommand = new Command(() => {
                model.SetSourceLang(SelectedSrcLang);
                model.SetTargetLang(SelectedDestLang);
                // по MVVM бизнес-логику во ViewModel использовать нельзя
                // это требование считаем бизнес-логикой:
                // - если язык исходного текста не выбран, 
                //   то перед переводом приложение должно попытаться определить его самостоятельно;
                DestText = model.Translate(SrcText);
                // определение языка происходит в модели, 
                // поэтому обновляем поле, 
                // чтобы отразить возможное изменение модели
                SelectedSrcLang = model.SourceLang;
                });
        }
        // TODO асинхронная работа
    }
}
