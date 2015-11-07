using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class WordSessionProvider: IWordSessionProvider
    {
        public WordSessionProvider(IWordStorage wordStorage, IStorage<WordResult> wordResultStorage, int sessionWordCount, byte customWordPercent)
        {
            this.WordStorage = wordStorage;
            this.WordResultStorage = wordResultStorage;
            this.sessionWordCount = sessionWordCount;
            this.customWordPercent = customWordPercent;
        }

        private int sessionWordCount;
        private int customWordPercent;

        protected IWordStorage WordStorage { get; set; }
        protected IStorage<WordResult> WordResultStorage { get; set; }

        public IEnumerable<Word> GetNextWords(Language language)
        {
            var list = WordStorage.GetWordsByLanguage(language)
                .GroupJoin(WordResultStorage.AllList, w => w.ID, wr => wr.Word.ID, (w, wrs) => new { Word = w, WordResults = wrs.DefaultIfEmpty() })
                .SelectMany(i => i.WordResults, (i, l) => new { Word = i.Word, WordResult = l })
                .Where(i => i.WordResult == null);

            var customList = list.Where(i => i.Word.UsingFrequencyNumber == null).Take(sessionWordCount * customWordPercent / 100).Select(i => i.Word);
            var frequencyList = list.Where(i => i.Word.UsingFrequencyNumber != null).Take(sessionWordCount - customList.Count()).Select(i => i.Word);

            var result = frequencyList.Union(customList);
            
            return result;
        }

        public IEnumerable<Word> GetWordsToRepeat(Language language)
        {
            var list = this.WordResultStorage.AllList
                .Where(wr => wr.LearningSchedule.Any(i => !i.IsShown && i.DateToShow <= DateTime.Now) && wr.Word.Language == language)
                .Select(wr => wr.Word);
            
            return list;
        }
    }
}
