using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public class WordResult
    {
        public Word Word { get; set; }

        public IEnumerable<ScheduleItem> LearningSchedule { get; set; }
    }
}
