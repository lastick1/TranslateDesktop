using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslateDesktop
{
    public interface ITranslateAPI
    {
        string GetLangs(string apiKey, string ui);
        string Detect(string apiKey, string text);
        string Translate(string apiKey, string text, string lang);
    }
}
