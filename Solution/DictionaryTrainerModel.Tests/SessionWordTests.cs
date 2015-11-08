using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class SessionWordTests
    {
        [TestMethod]
        public void AllTranslations()
        {
            var word = new SessionWord()
            {
                Word = new Word()
                {
                    Translations = 
                    { 
                        new Word() { Spelling = "a" },
                        new Word() { Spelling = "b" },
                        new Word() { Spelling = "c" }
                    }
                }
            };

            var actual = word.AllTranslations;
            var expected = "a, b, c";

            Assert.AreEqual(expected, actual);
        }
    }
}
