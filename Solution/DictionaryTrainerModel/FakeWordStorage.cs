using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainerModel
{
    public class FakeWordStorage: IWordStorage
    {
        public string Source { get; set; }
        
        public IList<Word> AllList
        {
            get 
            {
                var list = new List<Word>()
                {
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "the", UsingFrequencyNumber = 1, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "определенный (артикль)" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "and", UsingFrequencyNumber = 2, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "и" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "to", UsingFrequencyNumber = 3, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "к" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "I", UsingFrequencyNumber = 4, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "я" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "of", UsingFrequencyNumber = 5, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "от" }, new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "об" } },
                        Phrases = new List<string>() {"a leg of a table", "removal of item"}},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "have", UsingFrequencyNumber = 6, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "иметь" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "you", UsingFrequencyNumber = 7, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "ты" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "he", UsingFrequencyNumber = 8, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "он" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "it", UsingFrequencyNumber = 9, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "оно" } }},
                    new Word() {ID = Guid.NewGuid(), Language = DictionaryTrainerModel.Language.En, Spelling = "in", UsingFrequencyNumber = 10, 
                        Translations = new List<Word>() { new Word() {ID = Guid.NewGuid(), Language = Language.Rus, Spelling = "в" } }},
                };

                return list;
            }
        }

        public IEnumerable<Word> GetWordsByLanguage(Language language)
        {
            return this.AllList.Where(w => w.Language == language);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Reopen()
        {
            throw new NotImplementedException();
        }


        public bool IsWordAlreadyExists(Word word)
        {
            throw new NotImplementedException();
        }
    }
}
