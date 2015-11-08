using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class WordTests
    {
        [TestMethod]
        public void Constructor()
        {
            var word = new Word();

            Assert.IsNotNull(word.Translations);
            Assert.IsNotNull(word.Phrases);
        }

        [TestMethod]
        public void SavePointer()
        {
            var word = new Word();

            Assert.IsNotNull(word.SavePointer);
            Assert.IsInstanceOfType(word.SavePointer, typeof(WordSavePointer));
        }
    }
}
