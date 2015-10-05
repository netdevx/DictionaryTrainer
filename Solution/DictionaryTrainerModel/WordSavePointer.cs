using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class WordSavePointer: BaseSavePointer<Word>
    {
        public WordSavePointer(Word source) : base(source) { }

        protected override void CopyTo(Word source, Word copy)
        {
            copy.ID = source.ID;
            copy.Language = source.Language;
            copy.Spelling = source.Spelling;
            copy.UsingFrequencyNumber = source.UsingFrequencyNumber;
            copy.CreateDate = source.CreateDate;
            copy.Phrases = new List<string>();

            foreach (var phrase in source.Phrases)
                copy.Phrases.Add(phrase);

            copy.Translations = new List<Word>();
            foreach (var translation in source.Translations)
            {
                copy.Translations.Add(this.Clone(translation));
            }
        }
    }
}
