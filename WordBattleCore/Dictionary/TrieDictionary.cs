using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WordBattleCore.Dictionary
{
    public class TrieDictionary
    {
        public class TrieNode
        {
            string word;

            public string Word
            {
                get { return word; }
                set { word = value; }
            }

            TrieNode[] child;

            public TrieNode[] Child
            {
                get { return child; }
                set { child = value; }
            }

            public TrieNode ChildAt(char chr)
            {
                int childId = (int)chr - (int)'A';
                return child[childId];
            }

            public TrieNode() {
                child = new TrieNode[26];
                word = "";
            }
        }

        TrieNode rootTree;

        public TrieNode RootTree
        {
            get { return rootTree; }
        }

        private static TrieDictionary instance;

        public static TrieDictionary GetInstance()
        {
            if (instance == null)
                instance = new TrieDictionary();
            return instance;
        }

        private TrieDictionary()
        {
            rootTree = new TrieNode();
        }

        public void Load(string[] words) {
            rootTree = new TrieNode();

            foreach (var word in words) {
                AddWord(word);
                AddReversedWord(word);
            }
        }

        public void AddWord(string word)
        {
            // Make sure that word is upper case
            word = word.ToUpper();

            TrieNode node = rootTree;
            for (int index = 0; index < word.Length; index++)
            {
                int childId = (int)word[index] - (int)'A';
                if (node.Child[childId] == null)
                    node.Child[childId] = new TrieNode();
                node = node.Child[childId];
            }
            node.Word = word;
        }

        public void AddReversedWord(string word)
        {
            // Make sure that word is upper case
            word = word.ToUpper();

            TrieNode node = rootTree;
            for (int index = word.Length - 1; index >= 0; index--)
            {
                int childId = (int)word[index] - (int)'A';
                if (node.Child[childId] == null)
                    node.Child[childId] = new TrieNode();
                node = node.Child[childId];
            }
            node.Word = word;
        }
    }
}
