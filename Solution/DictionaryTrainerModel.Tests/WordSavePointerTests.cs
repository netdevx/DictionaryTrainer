using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class WordSavePointerTests
    {
        private Word CreateWord()
        {
            var word = new Word()
            {
                ID = Guid.NewGuid(),
                Language = Language.En,
                Spelling = "dream",
                UsingFrequencyNumber = 9,
                Translations = new List<Word>() { new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "мечтать" }, new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "мечта" } },
                Phrases = new List<string>() { "sweet dream" }
            };
            return word;
        }
        
        [TestMethod]
        public void Copy()
        {
            var savePointer = new WordSavePointer(this.CreateWord());
            var actual = new Word();
            var expected = savePointer.Entity;

            savePointer.CopyTo(savePointer.Entity, actual);

            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Language, actual.Language);
            Assert.AreEqual(expected.Spelling, actual.Spelling);
            Assert.AreEqual(expected.UsingFrequencyNumber, actual.UsingFrequencyNumber);

            Assert.AreEqual(expected.Phrases.Count, actual.Phrases.Count);
            Assert.AreEqual(expected.Phrases[0], actual.Phrases[0]);

            Assert.AreEqual(expected.Translations.Count, actual.Translations.Count);
            Assert.AreEqual(expected.Translations[0].ID, actual.Translations[0].ID);
            Assert.AreEqual(expected.Translations[1].ID, actual.Translations[1].ID);
        }

        
        [TestMethod]
        public void IsNotSavePoint()
        {
            var savePointer = new WordSavePointer(this.CreateWord());

            Assert.IsFalse(savePointer.IsSavePoint);
        }
        
        [TestMethod]
        public void MakeSavePoint()
        {
            var savePointer = new WordSavePointer(this.CreateWord());

            savePointer.MakeSavePoint();

            Assert.IsTrue(savePointer.IsSavePoint);
        }

        [TestMethod]
        [ExpectedException(typeof(DictionaryTrainerException))]
        public void MakeSecondSavePoint()
        {
            var savePointer = new WordSavePointer(this.CreateWord());

            savePointer.MakeSavePoint();
            savePointer.MakeSavePoint();

            Assert.IsTrue(savePointer.IsSavePoint);
        }

        [TestMethod]
        public void Rollback()
        {
            var savePointer = new WordSavePointer(this.CreateWord());
            var expected = savePointer.Entity.Spelling;

            savePointer.MakeSavePoint();
            savePointer.Entity.Spelling = "xxx";
            savePointer.RollbackToSavePoint();

            Assert.AreEqual(expected, savePointer.Entity.Spelling);
        }

        [TestMethod]
        public void DeleteSavePoint()
        {
            var savePointer = new WordSavePointer(this.CreateWord());

            savePointer.MakeSavePoint();
            savePointer.DeleteSavePoint();

            Assert.IsFalse(savePointer.IsSavePoint);
        }
    }
}
