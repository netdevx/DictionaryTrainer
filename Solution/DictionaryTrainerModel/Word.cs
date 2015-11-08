using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public class Word: Entity, ISavePointable
    {
        public Word()
        {
            this.Phrases = new List<string>();
            this.Translations = new List<Word>();
        }

        public Language Language { get; set; }

        public string Spelling { get; set; }

        public int? UsingFrequencyNumber { get; set; }

        public IList<string> Phrases { get; set; }

        public IList<Word> Translations { get; set; }

        [NonSerialized]
        private WordSavePointer wordSavePointer;
        public ISavePointer SavePointer
        {
            get
            {
                if (wordSavePointer == null)
                    wordSavePointer = new WordSavePointer(this);
                
                return this.wordSavePointer;
            }
        }
    }
}
