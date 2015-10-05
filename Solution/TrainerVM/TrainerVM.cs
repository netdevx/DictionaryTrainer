using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class TrainerVM : INotifyPropertyChanged
    {
        public MainVM MainVM { get; protected set; }

        private Trainer trainer;

        private Timer repetitionTimer;

        public TrainerVM(MainVM mainVM)
        {
            this.MainVM = mainVM;
            var wordProvider = new WordSessionProvider(this.MainVM.WordStorage, this.MainVM.WordResultStorage);
            this.trainer = new Trainer(this.MainVM.WordStorage, this.MainVM.WordResultStorage, Language.En, wordProvider, new ScheduleBuilder());

            this.StartNewLearningCmd = new Command(StartNewLearning);
            this.StartRepetitionCmd = new Command(StartRepetition);
            this.StartCheckingCmd = new Command(StartChecking);
            this.CheckWordCmd = new Command(CheckWord);
            this.ContinueAfterErrorCmd = new Command(ContinueAfterError);
            this.EditWordCmd = new Command(EditWord);
            this.StopLearningSessionCmd = new Command(StopLearningSession);

            this.repetitionTimer = new Timer(o => 
            { 
                if (!this.IsSessionInProcess)
                    this.StartRepetitionCmd.Execute(null); 
            }
            , null, 0, 15 * 1000);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private ReadOnlyObservableCollection<LearningWord> newWords;
        public ReadOnlyObservableCollection<LearningWord> NewWords
        {
            get { return newWords; }
            protected set
            {
                newWords = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("ToShowNewWords");
            }
        }

        public bool ToShowNewWords
        {
            get
            {
                return this.NewWords != null && this.NewWords.Any();
            }
        }

        private LearningWord currentWord;
        public LearningWord CurrentWord
        {
            get { return currentWord; }
            protected set
            {
                currentWord = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("IsCurrentWordVisible");
            }
        }

        public bool IsCurrentWordVisible
        {
            get
            {
                return this.CurrentWord != null && !this.IsWrongAnswer;
            }
        }

        private bool isWrongAnswer;
        public bool IsWrongAnswer
        {
            get { return isWrongAnswer; }
            protected set
            {
                isWrongAnswer = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged("IsCurrentWordVisible");
            }
        }
        private bool isExpectedOtherAnswer;
        public bool IsExpectedOtherAnswer
        {
            get { return isExpectedOtherAnswer; }
            set { isExpectedOtherAnswer = value; this.RaisePropertyChanged(); }
        }

        private object lockToken = new Object();
        private bool isSessionInProcess = false;
        public bool IsSessionInProcess
        {
            get 
            {
                lock (lockToken)
                {
                    return isSessionInProcess;
                }
            }
            protected set
            {
                lock (lockToken)
                {
                    isSessionInProcess = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ICommand StartNewLearningCmd { get; protected set; }
        private void StartNewLearning(object parameter)
        {
            this.IsSessionInProcess = true;
            trainer.StartNewLearning();
            this.NewWords = new ReadOnlyObservableCollection<LearningWord>(new ObservableCollection<LearningWord>(trainer.Session.Items));
        }

        public ICommand StartRepetitionCmd { get; protected set; }
        private void StartRepetition(object parameter)
        {
            this.IsSessionInProcess = true;
            trainer.StartRepetition();
            this.CurrentWord = trainer.Session.CurrentWord;
            if (this.CurrentWord == null)
                this.IsSessionInProcess = false;
        }

        public ICommand StartCheckingCmd { get; protected set; }
        private void StartChecking(object parameter)
        {
            this.NewWords = null;
            this.CurrentWord = trainer.Session.CurrentWord;
        }

        public ICommand CheckWordCmd { get; protected set; }
        private void CheckWord(object parameter)
        {
            var result = trainer.Session.Next();
            this.CurrentWord.Answer = String.Empty;
            this.CurrentWord = trainer.Session.CurrentWord;

            this.IsWrongAnswer = (result == LearningSession.NextResult.Wrong || result == LearningSession.NextResult.ExpectedOtherVariant);
            this.IsExpectedOtherAnswer = result == LearningSession.NextResult.ExpectedOtherVariant;
            this.IsSessionInProcess = result != LearningSession.NextResult.Finished;
        }

        public ICommand ContinueAfterErrorCmd { get; protected set; }
        private void ContinueAfterError(object parameter)
        {
            this.IsWrongAnswer = false;
        }

        public ICommand EditWordCmd { get; protected set; }
        private void EditWord(object word)
        {
            word = word as Word;
            if (word != null)
            {
                this.MainVM.OpenEditorCmd.Execute(word);
            }
        }

        public ICommand StopLearningSessionCmd { get; protected set; }
        private void StopLearningSession(object parameter)
        {
            this.IsSessionInProcess = false;
            this.IsWrongAnswer = false;
            this.CurrentWord = null;
            this.NewWords = null;
        }
    }
}
