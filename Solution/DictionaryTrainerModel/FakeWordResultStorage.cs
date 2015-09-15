using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainerModel
{
    public class FakeWordResultStorage: IStorage<WordResult>
    {
        public string Source { get; set; }

        public FakeWordResultStorage()
        {
            this.AllList = new List<WordResult>();
        }

        public IList<WordResult> AllList { get; protected set; }

        public void Save()
        {
            ;
        }

        public void Reopen()
        {
            throw new NotImplementedException();
        }
    }
}
