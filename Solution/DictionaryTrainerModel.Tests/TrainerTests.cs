using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

using AnSoft.DictionaryTrainer.Model;
using Moq;

namespace DictionaryTrainerModel.Tests
{
    [TestClass]
    public class TrainerTests
    {
        Mock<IWordSessionProvider> provider;
        Mock<IScheduleBuilder> scheduleBuilder;
        Mock<IWordStorage> wordStorage;
        Mock<IStorage<WordResult>> wordResultStorage;
        IList<WordResult> wordResults;

        IList<Word> engWordList;
        IList<Guid> guids;


        [TestInitialize]
        public void Init()
        {
            guids = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            engWordList = new List<Word>() { new Word() { ID = guids[0] }, new Word() { ID = guids[1] } };
            
            provider = new Mock<IWordSessionProvider>();
            provider.Setup(p => p.GetNextWords(It.Is<Language>(l => l == Language.En))).Returns(() => engWordList);
            provider.Setup(p => p.GetWordsToRepeat(It.Is<Language>(l => l == Language.En))).Returns(() => engWordList);

            wordStorage = new Mock<IWordStorage>();

            wordResults = new List<WordResult>()
            {
                new WordResult() 
                { 
                    Word = new Word() { ID = guids[1], Spelling = "two" }, 
                    LearningSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-2), IsShown = true, IsSuccessful = true },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = false, IsSuccessful = false },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(0), IsShown = false, IsSuccessful = false }
                    } 
                }
            };
            wordResultStorage = new Mock<IStorage<WordResult>>();
            wordResultStorage.Setup(s => s.AllList).Returns(() => new ReadOnlyCollection<WordResult>(wordResults));
            wordResultStorage.Setup(s => s.Add(It.IsAny<WordResult>())).Returns(new Func<WordResult, WordResult>(wr =>                 
            {
                wordResults.Add(wr);
                return wr;
            }));
            
            scheduleBuilder = new Mock<IScheduleBuilder>();
        }
        
        [TestMethod]
        public void StartNewLearning()
        {
            var trainer = new Trainer(wordStorage.Object, wordResultStorage.Object, Language.En, provider.Object, scheduleBuilder.Object);

            trainer.StartNewLearning();

            Assert.IsNotNull(trainer.Session);
            MyAssertions.CollectionAssert(engWordList, trainer.Session.Items, (w, sw) => w.ID == sw.Word.ID);
        }

        [TestMethod]
        public void StartRepetition()
        {
            var trainer = new Trainer(wordStorage.Object, wordResultStorage.Object, Language.En, provider.Object, scheduleBuilder.Object);

            trainer.StartRepetition();

            Assert.IsNotNull(trainer.Session);
            MyAssertions.CollectionAssert(engWordList, trainer.Session.Items, (w, sw) => w.ID == sw.Word.ID);
        }

        [TestMethod]
        public void WriteResults()
        {
            var session = new Mock<Session4Test>();
            var passedWords = new List<Word>() 
            { 
                new Word() { ID = guids[0], Spelling = "one" }, 
                new Word() { ID = guids[1], Spelling = "two" }
            };
            session.Setup(s => s.PassedWords).Returns(() => new ReadOnlyCollection<Word>(passedWords));
            var finishTime = DateTime.Now;
            session.Setup(s => s.FinishTime).Returns(finishTime);

            scheduleBuilder.Setup(b => b.GetSchedule(It.IsAny<DateTime>()))
                .Returns(() => new List<ScheduleItem>() { new ScheduleItem() { DateToShow = finishTime, IsShown = false, IsSuccessful = false } });

            var trainer = new Trainer4Test(wordStorage.Object, wordResultStorage.Object, Language.En, provider.Object, scheduleBuilder.Object);

            trainer.UpdateScheduleTest(session.Object);

            var expectedSchedule = new List<ScheduleItem>()
                    {
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-2), IsShown = true, IsSuccessful = true },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(-1), IsShown = true, IsSuccessful = true },
                        new ScheduleItem() { DateToShow = DateTime.Now.AddDays(0), IsShown = false, IsSuccessful = false }
                    };

            Assert.AreEqual(2, wordResults.Count);
            
            Assert.AreEqual(guids[1], wordResults[0].Word.ID);
            MyAssertions.CollectionAssert(expectedSchedule, wordResults[0].LearningSchedule, (e, a) => e.IsShown == a.IsShown && e.IsSuccessful == a.IsSuccessful);
            
            Assert.AreEqual(guids[0], wordResults[1].Word.ID);
            Assert.IsNotNull(wordResults[1].LearningSchedule);
            MyAssertions.CollectionAssert(scheduleBuilder.Object.GetSchedule(DateTime.Now), wordResults[1].LearningSchedule, (e, a) =>  e.DateToShow == a.DateToShow && e.IsShown == a.IsShown && e.IsSuccessful == a.IsSuccessful);

            Assert.IsNull(trainer.Session);
        }
    }

    public class Session4Test: LearningSession
    {
        public Session4Test(): base(new List<Word>(), new List<Word>()) { }
    }
    
    public class Trainer4Test: Trainer
    {
        public Trainer4Test(IWordStorage wordStorage, IStorage<WordResult> wordResultStorage, Language language, IWordSessionProvider wordSessionProvider, IScheduleBuilder scheduleBuilder) : base(wordStorage, wordResultStorage, language, wordSessionProvider, scheduleBuilder) { }
        
        public void UpdateScheduleTest(LearningSession session)
        {
            this.UpdateSchedule(session);
        }
    }
}
