using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AnSoft.DictionaryTrainer.Model
{
    public class WordStorage: Storage<Word>, IWordStorage
    {
        public WordStorage(string source): base(source) {}
        
        public IEnumerable<Word> GetWordsByLanguage(Language language)
        {
            return AllList.Where(w => w.Language == language);
        }


        public bool IsWordAlreadyExists(Word word)
        {
            return this.GetWordsByLanguage(word.Language).Any(w => w != word && String.Equals(w.Spelling, word.Spelling, StringComparison.InvariantCultureIgnoreCase));
        }

        public override void Save()
        {
            if (this.AllList.Any(word => this.AllList.Where(w => w != word && w.Language == word.Language && String.Equals(w.Spelling, word.Spelling, StringComparison.InvariantCultureIgnoreCase)).Any()))
                throw new ApplicationException("There are some repetitions in word list within the same language! It's impossible to save changes!");
            foreach (var w in this.AllList)
                if (w.IsSavePoint)
                    w.DeleteSavePoint();
            base.Save();
        }
    }
}
