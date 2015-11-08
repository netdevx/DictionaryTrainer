using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class SessionWord
    {
        public SessionWord()
        {
            this.TimesToShow = 1;
        }

        public Word Word { get; set; }

        public string AllTranslations
        {
            get
            {
                return String.Join(", ", this.Word.Translations.Select(t => t.Spelling));
            }
        }

        public int TimesToShow { get; set; }

        public string Answer { get; set; }
    }
}
