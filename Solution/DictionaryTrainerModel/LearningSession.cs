using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AnSoft.DictionaryTrainer.Model
{
    public class LearningSession
    {
        public LearningSession(IEnumerable<Word> words, IEnumerable<Word> allWords)
        {
            this.allWords = allWords;

            var learningWords = words.Select(w => new SessionWord() { Word = w}).ToArray();
            
            this.items = new LinkedList<SessionWord>(learningWords);
            this.roItems = new ReadOnlyCollection<SessionWord>(this.items.ToList());
            this.CurrentItem = this.items.First;

            this.passedWords = new List<Word>();
            this.roPassedWords = new ReadOnlyCollection<Word>(passedWords);

            this.StartTime = DateTime.Now;
        }

        private IEnumerable<Word> allWords;

        private LinkedList<SessionWord> items;
        private IReadOnlyCollection<SessionWord> roItems;
        public IReadOnlyCollection<SessionWord> Items
        {
            get { return roItems; }
        }
        
        private List<Word> passedWords;
        private IReadOnlyCollection<Word> roPassedWords;
        public virtual IReadOnlyCollection<Word> PassedWords
        {
            get { return roPassedWords; }
        }

        protected LinkedListNode<SessionWord> CurrentItem { get; set; }

        public SessionWord CurrentWord 
        {
            get { return this.CurrentItem != null ? this.CurrentItem.Value : null; }
        }

        public NextResult Next()
        {
            if (String.Equals(this.CurrentWord.Word.Spelling, this.CurrentWord.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                this.CurrentWord.TimesToShow--;
                var itemToDelete = this.CurrentWord.TimesToShow <= 0 ? this.CurrentItem : null;

                // it provides cycling walk of linked list
                this.CurrentItem = this.CurrentItem.Next ?? (this.items.First != itemToDelete ? this.items.First : null);
                if (itemToDelete != null)
                {
                    this.items.Remove(itemToDelete);
                    this.passedWords.Add(itemToDelete.Value.Word);
                }

                if (this.CurrentItem != null)
                    return NextResult.Correct;
                else
                {
                    this.FinishTime = DateTime.Now;

                    if (this.OnFinish != null)
                        this.OnFinish(this);

                    return NextResult.Finished;
                }
            }
            else
            {                
                var answer = this.allWords.FirstOrDefault(w => String.Equals(w.Spelling, this.CurrentWord.Answer, StringComparison.CurrentCultureIgnoreCase));
                
                if ((answer != null && answer.Translations.Any(t => this.CurrentWord.Word.Translations.Any(x => String.Equals(x.Spelling, t.Spelling, StringComparison.CurrentCultureIgnoreCase)))))
                {
                    return NextResult.ExpectedOtherVariant;
                }
                else
                {
                    this.CurrentWord.TimesToShow += 2;
                    return NextResult.Wrong;
                }
            }
        }

        public virtual DateTime StartTime { get; protected set; }

        public virtual DateTime FinishTime { get; protected set; }

        public event Action<LearningSession> OnFinish;

        public enum NextResult
        {
            Correct = 1,
            Wrong = 2,
            ExpectedOtherVariant = 3,
            Finished = 4
        }
    }
}
