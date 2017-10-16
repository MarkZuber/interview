using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConsoleApp2
{
    class Program
    {
        /*
         * This gives the following output in the perf test up to 20 character words (which can now be searched in about 1.86 seconds with the sorted-char trie and the memoization sub-string cache)
            For a (len=1, time=1ms: found 0 words)
            For ab (len=2, time=0ms: found 0 words)
            For abs (len=3, time=0ms: found 0 words)
            For abse (len=4, time=0ms: found 3 words)
            For absen (len=5, time=0ms: found 13 words)
            For absent (len=6, time=0ms: found 31 words)
            For absentm (len=7, time=0ms: found 53 words)
            For absentmi (len=8, time=0ms: found 99 words)
            For absentmin (len=9, time=1ms: found 108 words)
            For absentmind (len=10, time=5ms: found 214 words)
            For absentminde (len=11, time=14ms: found 294 words)
            For absentminded (len=12, time=67ms: found 347 words)
            For absentmindedn (len=13, time=107ms: found 351 words)
            For absentmindedne (len=14, time=155ms: found 369 words)
            For absentmindednes (len=15, time=503ms: found 468 words)
            For absentmindedness (len=16, time=578ms: found 497 words)
            For absentmindednessq (len=17, time=1547ms: found 498 words)
            For absentmindednessqq (len=18, time=1642ms: found 498 words)
            For absentmindednessqqq (len=19, time=1754ms: found 498 words)
            For absentmindednessqqqq (len=20, time=1860ms: found 498 words)
        */
        static void Main()
        {
            string wordsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "corncob_lowercase.txt");
            var englishWords = File.ReadAllLines(wordsPath);
            var wordDictionary = new TrieWordDictionary(englishWords);

            RunPerfTest(wordDictionary);
            RunGame(wordDictionary);
        }

        private static void RunGame(TrieWordDictionary wordDictionary)
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                Console.WriteLine("Enter characters to check.  empty to exit.");
                string upTiles = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(upTiles))
                {
                    keepGoing = false;
                }
                else
                {
                    ExecuteForWord(wordDictionary, upTiles, false);
                    Console.WriteLine();
                }
            }
        }

        private static void RunPerfTest(TrieWordDictionary wordDictionary)
        {
            string maxWord = "absentmindednessqqqq";
            for (int i = 1; i <= maxWord.Length; i++)
            {
                string curWord = maxWord.Substring(0, i);
                ExecuteForWord(wordDictionary, curWord, true);
            }
        }

        private static void ExecuteForWord(TrieWordDictionary wordDictionary, string upTiles, bool onlyShowCountOfWords)
        {
            var sw = Stopwatch.StartNew();
            var availableWords = wordDictionary.GetAvailableWordsForWord(upTiles);
            sw.Stop();
            string words = onlyShowCountOfWords ? $"found {availableWords.ToList().Count} words": string.Join(",", availableWords);
            Console.WriteLine($"For {upTiles} (len={upTiles.Length}, time={sw.ElapsedMilliseconds}ms: {words})");
        }
    }
}
