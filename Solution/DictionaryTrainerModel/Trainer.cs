using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class Trainer
    {
        private IWordStorage wordStorage;
        private IStorage<WordResult> wordResultStorage;

        public Trainer(IWordStorage wordStorage, IStorage<WordResult> wordResultStorage, Language language)
        {
            this.wordStorage = wordStorage;
            this.wordResultStorage = wordResultStorage;
            this.Language = language;
        }

        public Language Language { get; set; }
        
        public Mode SwitchedMode { get; set; }

        public LearningSession Start()
        {
            return null;
        }

        public LearningSession StartNewLearning()
        {
            var provider = new WordSessionProvider(this.wordStorage, this.wordResultStorage);
            var session = new LearningSession(provider.GetNextWords(this.Language), this.wordStorage);
            session.OnFinishHandler += UpdateSchedule;

            return session;
        }

        public LearningSession StartRepetition()
        {
            var provider = new WordSessionProvider(this.wordStorage, this.wordResultStorage);
            var session = new LearningSession(provider.GetWordsToRepeat(this.Language), this.wordStorage);
            session.OnFinishHandler += UpdateSchedule;

            return session;
        }

        private void UpdateSchedule(LearningSession session)
        {
            var scheduleBuilder = new ScheduleBuilder();
            
            foreach (var word in session.PassedWords)
            {
                var existingWordResult = wordResultStorage.AllList.Where(wr => wr.Word == word).FirstOrDefault();
                if (existingWordResult == null)
                    wordResultStorage.AllList.Add(new WordResult() { Word = word, LearningSchedule = scheduleBuilder.GetSchedule(session.FinishTime).ToList() });
                else
                {
                    var scheduleItem = existingWordResult.LearningSchedule.Where(i => i.IsShown == false).OrderBy(i => i.DateToShow).FirstOrDefault();
                    if (scheduleItem != null)
                    {
                        scheduleItem.IsShown = scheduleItem.IsSuccessful = true;
                    }
                }
            }
            wordResultStorage.Save();
        }

        public enum Mode
        {
            FromForeign = 1,
            FromNative = 2
        }
    }
}
