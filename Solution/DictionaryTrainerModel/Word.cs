using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnSoft.DictionaryTrainer.Model
{
    [Serializable]
    public class Word: IEquatable<Word>, ICloneable
    {
        public Word()
        {
            this.Phrases = new List<string>();
            this.Translations = new List<Word>();
        }

        public Guid ID { get; set; }

        public Language Language { get; set; }

        public string Spelling { get; set; }

        public int? UsingFrequencyNumber { get; set; }

        public IList<string> Phrases { get; set; }

        public IList<Word> Translations { get; set; }

        public DateTime CreateDate { get; set; }

        public bool Equals(Word other)
        {
            if (other == null)
                return false;
            else if (Object.ReferenceEquals(this, other))
                return true;
            else
                return this.ID == other.ID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Word;
            if (other == null)
                return false;
            else
                return Equals(other);
        }

        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        public void CopyTo(Word copy)
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
                copy.Translations.Add(translation.Clone() as Word);
            }
        }

        public object Clone()
        {
            var clone = new Word();
            this.CopyTo(clone);

            return clone;
        }

        private Word savePoint;

        public void MakeSavePoint()
        {
            if (savePoint == null)
                this.savePoint = this.Clone() as Word;
            else
                throw new ApplicationException("SavePoint already has been made for this object!");
        }

        public void DeleteSavePoint()
        {
            this.savePoint = null;
        }

        public void RollbackToSavePoint()
        {
            if (this.savePoint != null)
            {
                this.savePoint.CopyTo(this);
                this.savePoint = null;
            }
        }

        public bool IsSavePoint
        {
            get { return this.savePoint != null; }
        }
    }
}
