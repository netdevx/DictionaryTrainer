using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.Storage
{
    public class WordStorage: Storage<Word>, IWordStorage
    {
        public WordStorage(string source): base(source) {}
        
        public IEnumerable<Word> GetWordsByLanguage(Language language)
        {
            return AllList.Where(w => w.Language == language);
        }

        public override Word Add(Word item)
        {
            this.ThrowExceptionIfWordExists(item);

            this.ReplaceTranslationRepetitions(item);
            return base.Add(item);
        }

        private void ThrowExceptionIfWordExists(Word item)
        {
            if (this.IsWordAlreadyExists(item))
                throw new WordStorageException("The same word already exists in word base!");
        }

        private void ReplaceTranslationRepetitions(Word item)
        {
            for (int i = 0; i < item.Translations.Count; i++)
            {
                var existingWord = this.GetWordsByLanguage(item.Translations[i].Language).Where(w => String.Equals(w.Spelling, item.Translations[i].Spelling, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (existingWord != null)
                {
                    if (!ReferenceEquals(existingWord, item.Translations[i]))
                        item.Translations[i] = existingWord;
                }
                else
                {
                    this.Items.Add(item.Translations[i]);
                }
            }
        }

        public override Word Update(Word item)
        {
            this.ThrowExceptionIfWordExists(item);

            this.ReplaceTranslationRepetitions(item);            
            return base.Update(item);
        }

        public bool IsWordAlreadyExists(Word word)
        {
            return this.GetWordsByLanguage(word.Language).Any(w => w != word && String.Equals(w.Spelling, word.Spelling, StringComparison.InvariantCultureIgnoreCase));
        }

        public override void Save()
        {
            if (this.AllList.Any(word => this.AllList.Where(w => w != word && w.Language == word.Language && String.Equals(w.Spelling, word.Spelling, StringComparison.InvariantCultureIgnoreCase)).Any()))
                throw new WordStorageException("There are some repetitions in word list within the same language! It's impossible to save changes!");
            
            foreach (var w in this.AllList)
                if (w.SavePointer.IsSavePoint)
                    w.SavePointer.DeleteSavePoint();
            
            base.Save();
        }
    }
}
