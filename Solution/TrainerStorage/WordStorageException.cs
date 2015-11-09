using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Storage
{
    public class WordStorageException: ApplicationException
    {
        public WordStorageException(string message) : base(message) { }
    }
}
