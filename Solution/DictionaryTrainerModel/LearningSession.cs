using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnSoft.DictionaryTrainer.Model
{
    public class LearningSession
    {
        private IWordStorage wordStorage;

        public LearningSession(IEnumerable<Word> words, IWordStorage wordStorage)
        {
            this.wordStorage = wordStorage;
            //if (!words.Any())
            //    throw new Exception("There are no any words to start learning");

            var learningWords = words.Select(w => new LearningWord() { Word = w}).ToArray();
            this.Items = new LinkedList<LearningWord>(learningWords);
            this.passedWords = new List<Word>();
            this.CurrentItem = this.Items.First;

            this.StartTime = DateTime.Now;
        }

        public LinkedList<LearningWord> Items { get; protected set; }
        private IList<Word> passedWords;
        public IEnumerable<Word> PassedWords
        {
            get { return passedWords; }
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

                this.CurrentItem = this.CurrentItem.Next ?? (this.Items.First != itemToDelete ? this.Items.First : null);
                if (itemToDelete != null)
                {
                    this.Items.Remove(itemToDelete);
                    this.passedWords.Add(itemToDelete.Value.Word);
                }

                if (this.CurrentItem != null)
                    return NextResult.Correct;
                else
                {
                    this.FinishTime = DateTime.Now;

                    if (this.OnFinishHandler != null)
                        this.OnFinishHandler(this);

                    return NextResult.Finished;
                }
            }
            else
            {                
                var answer = this.wordStorage.AllList.FirstOrDefault(w => String.Equals(w.Spelling, this.CurrentWord.Answer, StringComparison.CurrentCultureIgnoreCase));
                
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

        public event Action<LearningSession> OnFinishHandler;

        public enum NextResult
        {
            Correct = 1,
            Wrong = 2,
            ExpectedOtherVariant = 3,
            Finished = 4
        }
    }
}
