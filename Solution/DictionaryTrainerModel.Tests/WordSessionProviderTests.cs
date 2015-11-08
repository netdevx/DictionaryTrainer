using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using AnSoft.DictionaryTrainer.Model;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class WordSessionProviderTests
    {
        private Guid[] guids = { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        [TestMethod]
        public void GetNextWords()
        {
            var allList = new List<Word>()
            {
                new Word() { ID = guids[0], Language = Language.En, UsingFrequencyNumber = 1 },
                new Word() { ID = guids[1], Language = Language.En },
                new Word() { ID = guids[2], Language = Language.En },
                new Word() { ID = guids[3], Language = Language.En },
                new Word() { ID = guids[4], Language = Language.En, UsingFrequencyNumber = 2 }
            };
            var resList = new List<WordResult>()
            {
                new WordResult() { Word = new Word() { ID = guids[3], Language = Language.En } },
                new WordResult() { Word = new Word() { ID = guids[4], Language = Language.En } },
            };
            
            var wordResultStorage = new Mock<IStorage<WordResult>>();
            wordResultStorage.Setup(s => s.AllList).Returns(() => resList);

            var wordStorage = new Mock<IWordStorage>();
            wordStorage.Setup(s => s.GetWordsByLanguage(It.Is<Language>(x => x == Language.En))).Returns(() => allList);

            var provider = new WordSessionProvider(wordStorage.Object, wordResultStorage.Object, 5, 50);

            var actual = provider.GetNextWords(Language.En).ToArray();

            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual(guids[0], actual[0].ID);
            Assert.AreEqual(guids[1], actual[1].ID);
            Assert.AreEqual(guids[2], actual[2].ID);
        }

        
        [TestMethod]
        public void GetWordsToRepeat()
        {
            var words = new List<WordResult>()
            {
                new WordResult() { Word = new Word() { ID = guids[0], Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = true },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = guids[1], Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = guids[2], Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(2), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = guids[3], Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-2), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = guids[4], Language = Language.Ukr }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-2), IsShown = false }
                    }
                }
            };

            var wordResultStorage = new Mock<IStorage<WordResult>>();
            wordResultStorage.Setup(s => s.AllList).Returns(() => words);
            var wordStorage = new Mock<IWordStorage>();

            var provider = new WordSessionProvider(wordStorage.Object, wordResultStorage.Object, 5, 50);

            var actual = provider.GetWordsToRepeat(Language.En);

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(guids[1], actual.First().ID);
            Assert.AreEqual(guids[3], actual.Last().ID);
        }
    }
}
