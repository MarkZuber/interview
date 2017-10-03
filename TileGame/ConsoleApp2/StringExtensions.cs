using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    public static class StringExtensions
    {
        public static string AsSortedByChars(this string word)
        {
            char[] wordArray = word.ToCharArray();
            Array.Sort(wordArray);
            return new string(wordArray);
        }

        public static IEnumerable<string> GetSortedPermutations(this string word)
        {
            var setPerms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            GetPermsRecurse(word.AsSortedByChars(), setPerms);
            return setPerms;
        }

        private static void GetPermsRecurse(string word, HashSet<string> setPerms)
        {
            setPerms.Add(word);
            if (word.Length > 2)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    string subWord = word.Remove(i, 1);
                    setPerms.Add(subWord);
                    GetPermsRecurse(subWord, setPerms);
                }
            }
        }
    }
}
