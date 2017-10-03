using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp2
{
    public class WordDictionary
    {
        private readonly Dictionary<string, List<string>> _sortedCharsToWords = new Dictionary<string, List<string>>();

        public WordDictionary(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                string sortedWord = word.AsSortedByChars();

                if (!_sortedCharsToWords.TryGetValue(sortedWord, out var availableWords))
                {
                    availableWords = new List<string>();
                    _sortedCharsToWords[sortedWord] = availableWords;
                }
                availableWords.Add(word);
            }
        }

        public IEnumerable<string> GetAvailableWordsForWord(string word)
        {
            return GetAvailableWordsForChars(word.ToCharArray());
        }

        public IEnumerable<string> GetAvailableWordsForChars(IEnumerable<char> chars)
        {
            HashSet<string> wordSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            string charsAsWord = new string(chars.ToArray());
            foreach (string permSortedWord in charsAsWord.GetSortedPermutations())
            {
                if (_sortedCharsToWords.TryGetValue(permSortedWord, out var availableWords))
                {
                    foreach (string word in availableWords)
                    {
                        wordSet.Add(word);
                    }
                }
            }

            return wordSet;
        }
    }
}
