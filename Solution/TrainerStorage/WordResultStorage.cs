using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.Storage
{
    public class WordResultStorage: Storage<WordResult>
    {
        public WordResultStorage(string source) : base(source) { }
    }
}
