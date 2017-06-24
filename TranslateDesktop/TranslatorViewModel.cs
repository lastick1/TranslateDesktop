using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TranslateDesktop
{
    class TranslatorViewModel: DependencyObject
    {
        public KeyValuePair<string, string> SelectedSrcLang
        {
            get { return (KeyValuePair<string, string>)GetValue(SelectedSrcLangProperty); }
            set { SetValue(SelectedSrcLangProperty, value); }
        }

        public static readonly DependencyProperty SelectedSrcLangProperty =
            DependencyProperty.Register("SelectedSrcLang", typeof(KeyValuePair<string, string>), typeof(TranslatorViewModel), new PropertyMetadata(null));



        public KeyValuePair<string, string> SelectedDestLang
        {
            get { return (KeyValuePair<string, string>)GetValue(SelectedDestLangProperty); }
            set {
                SetValue(SelectedDestLangProperty, value);
                model.TargetLang = value.Key;
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

        private ITranslatorModel model = new TranslatorModel();
        public TranslatorViewModel()
        {
            SrcLangs = model.Langs;
            SelectedDestLang = model.Langs.First((x) => x.Key == "en");
            DestLangs = model.Langs;
            TranslateCommand = new Command(() => {
                SelectedSrcLang = model.DetectLang(SrcText);
                DestText = model.Translate(SrcText);
                });
        }
        // TODO асинхронная работа
    }
}
