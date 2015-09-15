using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public interface IWordStorage: IStorage<Word>
    {
        IEnumerable<Word> GetWordsByLanguage(Language language);

        bool IsWordAlreadyExists(Word word);
    }
}
