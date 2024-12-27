using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VulgarLangHelper.Parsers;
using VulgarLangHelper.Commands;

namespace VulgarLangHelper.ViewModels
{
    public class DictionaryEditorViewModel : INotifyPropertyChanged
    {
        public string RawInput { get; set; }
        public ObservableCollection<DictionaryItem> ParsedItems { get; set; }
        public ObservableCollection<ReplacementHistoryItem> ReplacementHistory { get; set; }

        public string TargetCharacter { get; set; }
        public string ReplacementCharacters { get; set; }

        private string _output = "test"; 
        public string Output
        {
            get => _output;
            set
            {
                if (_output != value)
                {
                    _output = value;
                    OnPropertyChanged(nameof(Output)); // Notify the UI
                }
            }
        }

        public ICommand ParseCommand { get; }
        public ICommand ReplaceCharacterCommand { get; }

        public DictionaryEditorViewModel()
        {
            ParsedItems = new ObservableCollection<DictionaryItem>();
            ReplacementHistory = new ObservableCollection<ReplacementHistoryItem>();
            ParseCommand = new RelayCommand(_ => ParseDictionary());
            ReplaceCharacterCommand = new RelayCommand(_ => ReplaceCharacter());
        }

        private void ParseDictionary()
        {
            try
            {
                var items = VocabularyParser.ParseVocabularyDictionary(RawInput);
                ParsedItems.Clear();
                foreach (var item in items)
                {
                    ParsedItems.Add(item);
                }

                UpdateOutput(); // Update the output whenever the dictionary is parsed
            }
            catch (FormatException ex)
            {
                // Handle invalid input
            }
        }
        private void ReplaceCharacter()
        {
            if (string.IsNullOrWhiteSpace(TargetCharacter) || string.IsNullOrWhiteSpace(ReplacementCharacters))
                return;

            var replacements = ReplacementCharacters.Split(',').Select(c => c.Trim()).ToArray();
            var random = new Random();

            foreach (var item in ParsedItems)
            {
                if (item.IpaPronunciation.Contains(TargetCharacter))
                {
                    var originalPronunciation = item.IpaPronunciation;
                    string newPronunciation = null;
                    bool replacementFound = false;

                    // Try all replacements
                    foreach (var replacement in replacements.OrderBy(_ => random.Next()))
                    {
                        newPronunciation = originalPronunciation.Replace(TargetCharacter, replacement);

                        // Check if the new pronunciation is unique
                        if (!ParsedItems.Any(i => i.IpaPronunciation == newPronunciation))
                        {
                            replacementFound = true;
                            break; // Found a valid replacement, exit the loop
                        }
                    }

                    // If no valid replacement is found, skip this word
                    if (!replacementFound)
                        continue;

                    // Update the item's pronunciation
                    item.IpaPronunciation = newPronunciation;

                    // Add to replacement history
                    ReplacementHistory.Add(new ReplacementHistoryItem
                    {
                        Translation = item.EnglishTranslation,
                        TypeOfSpeech = item.TypeOfSpeech,
                        Original = originalPronunciation,
                        Updated = newPronunciation
                    });
                }
            }

            UpdateOutput(); // Update the output after replacements
        }
        /*
                private void ReplaceCharacter()
                {
                    if (string.IsNullOrWhiteSpace(TargetCharacter) || string.IsNullOrWhiteSpace(ReplacementCharacters))
                        return;

                    var replacements = ReplacementCharacters.Split(',').Select(c => c.Trim()).ToArray();
                    var random = new Random();

                    foreach (var item in ParsedItems)
                    {
                        if (item.IpaPronunciation.Contains(TargetCharacter))
                        {
                            var originalPronunciation = item.IpaPronunciation;
                            var newPronunciation = originalPronunciation.Replace(
                                TargetCharacter,
                                replacements[random.Next(replacements.Length)]
                            );

                            if (ParsedItems.Any(i => i.IpaPronunciation == newPronunciation))
                                continue;

                            item.IpaPronunciation = newPronunciation;

                            ReplacementHistory.Add(new ReplacementHistoryItem
                            {
                                Translation = item.EnglishTranslation,
                                TypeOfSpeech = item.TypeOfSpeech,
                                Original = originalPronunciation,
                                Updated = newPronunciation
                            });
                        }
                    }

                    UpdateOutput(); // Update the output after replacements
                }*/

        private void UpdateOutput()
        {
            var sb = new StringBuilder();
            foreach (var item in ParsedItems)
            {
                sb.AppendLine($"{item.EnglishTranslation} : {item.TypeOfSpeech} = {item.IpaPronunciation}");
            }
            Output = sb.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
