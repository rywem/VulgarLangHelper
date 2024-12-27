using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulgarLangHelper
{
    public class DictionaryItem
    {
        public string Spelling { get; set; } // Not used in Vocabulary Dictionary format
        public string IpaPronunciation { get; set; }
        public string TypeOfSpeech { get; set; }
        public string EnglishTranslation { get; set; }
    }

}
