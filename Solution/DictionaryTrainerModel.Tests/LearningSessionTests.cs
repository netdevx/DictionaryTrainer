using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class LearningSessionTests
    {

        private IList<Word> words = new List<Word>()
            {
                new Word() 
                { 
                    ID = Guid.NewGuid(), Language = Language.En, Spelling = "one", 
                    Translations = new List<Word>() 
                    { 
                        new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "один" }
                    }
                },
                new Word() 
                { 
                    ID = Guid.NewGuid(), Language = Language.En, Spelling = "two", 
                    Translations = new List<Word>() 
                    { 
                        new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "два" }
                    }
                },
                new Word() 
                { 
                    ID = Guid.NewGuid(), Language = Language.En, Spelling = "single", 
                    Translations = new List<Word>() 
                    { 
                        new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "один" },
                        new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "единственный" }
                    }
                }
            };

        [TestMethod]
        public void Constructor()
        {
            var session = new LearningSession(this.words, this.words);

            MyAssertions.CollectionAssert(this.words, session.Items, (e, a) => e.ID == a.Word.ID);
            Assert.AreEqual(this.words[0].ID, session.CurrentWord.Word.ID);
            Assert.IsNotNull(session.PassedWords);
            Assert.IsNotNull(session.StartTime);
        }

        [TestMethod]
        public void NextCorrect()
        {
            var session = new LearningSession(this.words, this.words);

            session.CurrentWord.Answer = "one";
            var actual = session.Next();

            Assert.AreEqual(LearningSession.NextResult.Correct, actual);
            Assert.IsNull(session.Items.Where(w => w.Word.Spelling == "one").FirstOrDefault());
            Assert.IsNotNull(session.PassedWords.Where(w => w.Spelling == "one").FirstOrDefault());
        }

        [TestMethod]
        public void NextWrong()
        {
            var session = new LearningSession(this.words, this.words);

            session.CurrentWord.Answer = "two";
            var actual = session.Next();
            var currentWord = session.Items.Where(w => w.Word.Spelling == "one").FirstOrDefault();

            Assert.AreEqual(LearningSession.NextResult.Wrong, actual);
            Assert.IsNotNull(currentWord);
            Assert.AreEqual(3, currentWord.TimesToShow);
            Assert.AreEqual(session.CurrentWord.Word.ID, currentWord.Word.ID);
        }

        [TestMethod]
        public void NextExpactedOtherVariant()
        {
            var session = new LearningSession(this.words, this.words);

            session.CurrentWord.Answer = "one";
            session.Next();
            session.CurrentWord.Answer = "two";
            session.Next();

            session.CurrentWord.Answer = "one";
            var actual = session.Next();
            var currentWord = session.Items.Where(w => w.Word.Spelling == "single").FirstOrDefault();

            Assert.AreEqual(LearningSession.NextResult.ExpectedOtherVariant, actual);
            Assert.IsNotNull(currentWord);
            Assert.AreEqual(1, currentWord.TimesToShow);
            Assert.AreEqual(session.CurrentWord.Word.ID, currentWord.Word.ID);
        }

        [TestMethod]
        public void NextFinished()
        {
            var session = new LearningSession(this.words, this.words);
            bool isFinished = false;
            session.OnFinish += (s) => isFinished = true;

            session.CurrentWord.Answer = "one";
            session.Next();
            session.CurrentWord.Answer = "two";
            session.Next();

            session.CurrentWord.Answer = "single";
            var actual = session.Next();

            Assert.AreEqual(LearningSession.NextResult.Finished, actual);
            Assert.IsNull(session.CurrentWord);
            Assert.AreEqual(0, session.Items.Count);
            Assert.AreEqual(3, session.PassedWords.Count);
            Assert.AreEqual(true, isFinished);
        }

        [TestMethod]
        public void NextCycling()
        {
            var session = new LearningSession(this.words, this.words);

            session.CurrentWord.Answer = "on";
            session.Next();
            session.CurrentWord.Answer = "one";
            session.Next();
            session.CurrentWord.Answer = "two";
            session.Next();
            session.CurrentWord.Answer = "single";
            session.Next();

            Assert.IsNotNull(session.CurrentWord);
            Assert.AreEqual(words.Where(w => w.Spelling == "one").FirstOrDefault().ID, session.CurrentWord.Word.ID);
        }
    }
}
