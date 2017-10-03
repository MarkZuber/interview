using System;
using System.IO;
using System.Reflection;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string wordsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "corncob_lowercase.txt");
            var englishWords = File.ReadAllLines(wordsPath);
            var wordDictionary = new WordDictionary(englishWords);

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
                    var availableWords = wordDictionary.GetAvailableWordsForWord(upTiles);
                    string joined = string.Join(",", availableWords);
                    Console.WriteLine($"For {upTiles}: {joined}");
                    Console.WriteLine();
                }
            }
        }
    }
}
