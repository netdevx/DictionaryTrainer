using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public interface ISavePointer
    {
        void MakeSavePoint();

        void DeleteSavePoint();

        void RollbackToSavePoint();

        bool IsSavePoint { get; }
    }
}
