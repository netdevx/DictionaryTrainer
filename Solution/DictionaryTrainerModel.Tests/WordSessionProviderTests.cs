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
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        Guid id3 = Guid.NewGuid();
        Guid id4 = Guid.NewGuid();
        Guid id5 = Guid.NewGuid();

        [TestMethod]
        public void GetNextWords()
        {
            var allList = new List<Word>()
            {
                new Word() { ID = id1, Language = Language.En, UsingFrequencyNumber = 1 },
                new Word() { ID = id2, Language = Language.En },
                new Word() { ID = id3, Language = Language.En },
                new Word() { ID = id4, Language = Language.En },
                new Word() { ID = id5, Language = Language.En, UsingFrequencyNumber = 2 }
            };
            var resList = new List<WordResult>()
            {
                new WordResult() { Word = new Word() { ID = id4, Language = Language.En } },
                new WordResult() { Word = new Word() { ID = id5, Language = Language.En } },
            };
            
            var wordResultStorage = new Mock<IStorage<WordResult>>();
            wordResultStorage.Setup(s => s.AllList).Returns(() => resList);

            var wordStorage = new Mock<IWordStorage>();
            wordStorage.Setup(s => s.GetWordsByLanguage(It.Is<Language>(x => x == Language.En))).Returns(() => allList);

            var provider = new WordSessionProvider(wordStorage.Object, wordResultStorage.Object, 5, 50);

            var actual = provider.GetNextWords(Language.En).ToArray();

            Assert.AreEqual(3, actual.Count());
            Assert.AreEqual(id1, actual[0].ID);
            Assert.AreEqual(id2, actual[1].ID);
            Assert.AreEqual(id3, actual[2].ID);
        }


        [TestMethod]
        public void GetWordsToRepeat()
        {
            var words = new List<WordResult>()
            {
                new WordResult() { Word = new Word() { ID = id1, Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = true },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = id2, Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = id3, Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(2), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = id4, Language = Language.En }, LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-2), IsShown = false }
                    }
                },
                new WordResult() { Word = new Word() { ID = id5, Language = Language.Ukr }, LearningSchedule = new List<ScheduleItem>()
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
            Assert.AreEqual(id2, actual.First().ID);
            Assert.AreEqual(id4, actual.Last().ID);
        }
    }
}
