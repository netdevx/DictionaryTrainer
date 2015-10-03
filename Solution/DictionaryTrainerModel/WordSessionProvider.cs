using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class WordSessionProvider
    {
        private const int cnstSessionWordCount = 15;
        private const int cnstAddedWordCount = 7;

        protected IWordStorage WordStorage { get; set; }
        protected IStorage<WordResult> WordResultStorage { get; set; }

        public WordSessionProvider(IWordStorage wordStorage, IStorage<WordResult> wordResultStorage)
        {
            this.WordStorage = wordStorage;
            this.WordResultStorage = wordResultStorage;
        }

        public IEnumerable<Word> GetNextWords(Language language)
        {
            //var query = from w in wordStorage.GetWordsByLanguage(language)
            //            join wr in wordResultStorage.AllList on w.ID equals wr.Word.ID into gl
            //            from res in gl.DefaultIfEmpty()
            //            select new { Word = w, WordResult = (res == null ? null : res) };

            var list = WordStorage.GetWordsByLanguage(language)
                .GroupJoin(WordResultStorage.AllList, w => w.ID, wr => wr.Word.ID, (w, wrs) => new { Word = w, WordResults = wrs.DefaultIfEmpty() })
                .SelectMany(i => i.WordResults, (i, l) => new { Word = i.Word, WordResult = l })
                .Where(i => i.WordResult == null);

            var addedList = list.Where(i => i.Word.UsingFrequencyNumber == null)./*OrderBy(i => i.Word.ID).*/Take(cnstAddedWordCount).Select(i => i.Word);
            var frequencyList = list.Where(i => i.Word.UsingFrequencyNumber != null)./*OrderBy(i => i.Word.ID).*/Take(cnstSessionWordCount - addedList.Count()).Select(i => i.Word);

            var result = frequencyList.Union(addedList);
            
            return result;
        }

        public IEnumerable<Word> GetWordsToRepeat(Language language)
        {
            var list = this.WordResultStorage.AllList.Where(wr => wr.LearningSchedule.Any(i => !i.IsShown && i.DateToShow <= DateTime.Now)).Select(wr => wr.Word);
            
            return list;
        }
    }
}
