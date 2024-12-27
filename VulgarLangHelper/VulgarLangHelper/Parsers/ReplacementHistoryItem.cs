using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulgarLangHelper.Parsers
{
    public class ReplacementHistoryItem
    {
        public string Translation { get; set; }
        public string TypeOfSpeech { get; set; }
        public string Original { get; set; }
        public string Updated { get; set; }
    }
}
