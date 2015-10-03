using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public class Word: SavePointEntity<Word>
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

        public override void CopyTo(Word copy)
        {
            copy.ID = this.ID;
            copy.Language = this.Language;
            copy.Spelling = this.Spelling;
            copy.UsingFrequencyNumber = this.UsingFrequencyNumber;
            copy.CreateDate = this.CreateDate;
            copy.Phrases = new List<string>();

            foreach (var phrase in this.Phrases)
                copy.Phrases.Add(phrase);

            copy.Translations = new List<Word>();            
            foreach(var translation in this.Translations)
            {
                copy.Translations.Add(translation.Clone());
            }
        }
    }
}
