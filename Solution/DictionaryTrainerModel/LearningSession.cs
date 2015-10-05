using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class LearningSession
    {
        private IEnumerable<Word> wordStorage;

        public LearningSession(IEnumerable<Word> words, IEnumerable<Word> wordStorage)
        {
            this.wordStorage = wordStorage;
            //if (!words.Any())
            //    throw new Exception("There are no any words to start learning");

            var learningWords = words.Select(w => new LearningWord() { Word = w}).ToArray();
            this.items = new LinkedList<LearningWord>(learningWords);            
            this.passedWords = new List<Word>();            
            this.CurrentItem = this.items.First;

            this.StartTime = DateTime.Now;
        }

        private LinkedList<LearningWord> items;
        public IReadOnlyCollection<LearningWord> Items
        {
            get { return items.ToList().AsReadOnly(); }
        }
        
        private List<Word> passedWords;
        public IReadOnlyCollection<Word> PassedWords 
        {
            get { return passedWords.AsReadOnly(); }
        }

        protected LinkedListNode<LearningWord> CurrentItem { get; set; }

        public LearningWord CurrentWord 
        {
            get { return this.CurrentItem != null ? this.CurrentItem.Value : null; }
        }

        public NextResult Next()
        {
            if (String.Equals(this.CurrentWord.Word.Spelling, this.CurrentWord.Answer, StringComparison.CurrentCultureIgnoreCase))
            {
                this.CurrentWord.TimesToShow--;
                var itemToDelete = this.CurrentWord.TimesToShow <= 0 ? this.CurrentItem : null;

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
                var answer = this.wordStorage.FirstOrDefault(w => String.Equals(w.Spelling, this.CurrentWord.Answer, StringComparison.CurrentCultureIgnoreCase));
                
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

        public DateTime StartTime { get; protected set; }

        public DateTime FinishTime { get; protected set; }

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
