using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VulgarLangHelper.Parsers
{
    public class VocabularyParser
    {
        public static List<DictionaryItem> ParseVocabularyDictionary(string input)
        {
            var dictionaryItems = new List<DictionaryItem>();

            // Split input into lines
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // Split each line into parts based on format
                var parts = line.Split(new[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 3)
                {
                    var englishTranslations = parts[0].Trim(); // Part before ':'
                    var typeOfSpeech = parts[1].Trim(); // Part between ':' and '='
                    var pronunciation = parts[2].Trim(); // Part after '='

                    dictionaryItems.Add(new DictionaryItem
                    {
                        Spelling = null, // Not used in this format
                        EnglishTranslation = englishTranslations,
                        TypeOfSpeech = typeOfSpeech,
                        IpaPronunciation = pronunciation
                    });
                }
                else
                {
                    throw new FormatException($"Invalid line format: {line}");
                }
            }

            return dictionaryItems;
        }
    }
}
