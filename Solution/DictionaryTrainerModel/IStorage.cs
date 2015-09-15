using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public interface IStorage<T> where T: class
    {
        string Source { get; set; }

        IList<T> AllList { get; }

        void Save();

        void Reopen();
    }
}
