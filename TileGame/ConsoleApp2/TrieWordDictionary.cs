using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    /// <summary>
    /// This keeps a dictionary of words in a Trie where the words are stored in character-sorted order instead of the actual word order.
    /// For example, 'chicken' would be stored as 'ccehikn' in the ordering through the trie and the word termination value at the end would contain the actual (non-char-sorted) word, e.g. "chicken"
    /// </summary>
    public class TrieWordDictionary
    {
        private class TrieNode
        {
            private readonly Dictionary<char, TrieNode> _children = new Dictionary<char, TrieNode>();

            public TrieNode()
            {
                IsRoot = true;
            }

            public TrieNode(char c)
            {
                Character = c;
            }

            // These are private since I don't really need them for this exercise, but they make sense to be in the data structure for debugging the value at the current node.
            private char Character { get; }
            private bool IsRoot { get; }

            public bool IsWordTermination => !string.IsNullOrEmpty(WordTermination);
            public string WordTermination { get; set; }

            public bool TryGetChild(char c, out TrieNode childNode)
            {
                return _children.TryGetValue(c, out childNode);
            }

            public void AddChild(TrieNode child)
            {
                if (_children.ContainsKey(child.Character))
                {
                    throw new InvalidOperationException($"child with this character already exists {child.Character}");
                }

                _children[child.Character] = child;
            }
        }

        private readonly TrieNode _rootNode = new TrieNode();

        // Note that this will probably get very large over a long period of time if the game was used for many many words.
        // I'd want to monitor the growth of this cache and possibly flush it at some capacity.  But for the sake of this exercise,
        // it's reasonable.
        private readonly Dictionary<string, IEnumerable<string>> _tilesCache = new Dictionary<string, IEnumerable<string>>();

        public TrieWordDictionary(IEnumerable<string> words)
        {
            AddWords(words);
        }

        public void AddWords(IEnumerable<string> words)
        {
            foreach (string word in words)
            {
                AddWord(word);
            }
        }

        /// <summary>
        /// Add a word into the Trie in character-sorted order for more efficient lookup for our tile game and the ability to more easily cache clustered characters for re-use and to avoid repeat comparisons and recursion.
        /// </summary>
        /// <param name="word"></param>
        public void AddWord(string word)
        {
            var curNode = _rootNode;

            string sortedWord = word.AsSortedByChars();

            foreach (char c in sortedWord)
            {
                if (!curNode.TryGetChild(c, out TrieNode childNode))
                {
                    childNode = new TrieNode(c);
                    curNode.AddChild(childNode);
                }
                curNode = childNode;
            }
            curNode.WordTermination = word;
        }

        public IEnumerable<string> GetAvailableWordsForWord(string upTiles)
        {
            return RecurseGetAvailableWordsForWord(upTiles);
        }

        private IEnumerable<string> RecurseGetAvailableWordsForWord(string upTiles)
        {
            var words = new HashSet<string>();

            for (int i = 0; i < upTiles.Length; i++)
            {
                string subTiles = upTiles.Remove(i, 1);
                if (subTiles.Length > 2)
                {
                    if (_tilesCache.TryGetValue(subTiles, out var subTileWords))
                    {
                        foreach (var w in subTileWords)
                        {
                            words.Add(w);
                        }
                    }
                    else
                    {
                        var availWords = RecurseGetAvailableWordsForWord(subTiles);

                        var allWords = new List<string>();
                        allWords.AddRange(availWords);
                        allWords.AddRange(GetAvailableWordsFromTrie(subTiles));

                        _tilesCache[subTiles] = allWords;
                        foreach (var w in allWords)
                        {
                            words.Add(w);
                        }
                    }
                }
            }

            return words.AsEnumerable();
        }

        private IEnumerable<string> GetAvailableWordsFromTrie(string upTiles)
        {
            var availableWords = new HashSet<string>();
            string sortedTiles = upTiles.AsSortedByChars();
            var curNode = _rootNode;
            foreach (char c in sortedTiles)
            {
                if (curNode.TryGetChild(c, out TrieNode childNode))
                {
                    if (childNode.IsWordTermination)
                    {
                        availableWords.Add(childNode.WordTermination);
                    }
                    curNode = childNode;
                }
            }
            return availableWords;
        }
    }
}
