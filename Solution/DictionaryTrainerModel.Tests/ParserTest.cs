using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class ParserTest
    {
        private const string storageFileNameTemplate = "c:\\tmp\\{0}.dat";

        [TestMethod]
        public void ParseFile()
        {
            var parser = new WordsBaseParser();
            var words = parser.ExtractWordsFromFile("c:\\tmp\\words.txt");

            var wordStorage = new WordStorage(String.Format(storageFileNameTemplate, "dictionary"));
            //var wordResultStorage = new WordResultStorage(String.Format(storageFileNameTemplate, "results"));

            wordStorage.AllList.Clear();
            foreach (var w in words)
                wordStorage.AllList.Add(w);
            wordStorage.Save();
        }
    }
}
