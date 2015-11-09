using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class DictionaryTrainerException: ApplicationException
    {
        public DictionaryTrainerException(string message) : base(message) { }
    }
}
