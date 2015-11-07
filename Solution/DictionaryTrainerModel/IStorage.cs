using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public interface IStorage<T>
    {
        string Source { get; set; }

        IReadOnlyCollection<T> AllList { get; }

        T Add(T item);

        T Update(T item);

        bool Delete(T item);

        void Save();

        void Reopen();
    }
}
