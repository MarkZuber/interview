using System;

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
    }
}
