using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public class ScheduleItem
    {
        public DateTime DateToShow { get; set; }

        public bool IsShown { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
