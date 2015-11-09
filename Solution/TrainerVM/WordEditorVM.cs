using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class WordEditorVM : INotifyPropertyChanged
    {        
        public WordEditorVM(DictionaryEditorVM parentVM, Word word)
        {
            this.Word = word;
            this.DictionaryEditorVM = parentVM;
            this.wordStorage = DIContainer.Instance.Get<IWordStorage>();

            if (this.Word == null)
            {
                this.Word = new Word() { ID = Guid.NewGuid(), Language = Language.En };
                //this.Word.Translations.Add(new Word() { ID = Guid.NewGuid(), Language = Language.Rus });
                this.isNewWord = true;
            }
            else
                this.Word.SavePointer.MakeSavePoint();

            this.SaveTranslationCmd = new Command(SaveTranslation);
            this.AddTranslationCmd = new Command(AddTranslation);
            this.DeleteTranslationCmd = new Command(DeleteTranslation);
            this.MoveTranslationUpCmd = new Command(MoveTranslationUp);
            this.MoveTranslationDownCmd = new Command(MoveTranslationDown);

            this.SavePhraseCmd = new Command(SavePhrase);
            this.AddPhraseCmd = new Command(AddPhrase);
            this.DeletePhraseCmd = new Command(DeletePhrase);

            this.SaveCmd = new Command(Save);
            this.CancelCmd = new Command(Cancel);
        }

        public DictionaryEditorVM DictionaryEditorVM { get; protected set; }

        private IWordStorage wordStorage;

        private bool isNewWord = false;

        private Word word;
        public Word Word
        {
            get { return word; }
            set 
            { 
                word = value;
                this.RaisePropertyChanged();
            }
        }

        private int selectedTranslationIndex;
        public int SelectedTranslationIndex
        {
            get { return selectedTranslationIndex; }
            set { selectedTranslationIndex = value; RaisePropertyChanged(); }
        }

        private int selectedPhraseIndex;
        public int SelectedPhraseIndex
        {
            get { return selectedPhraseIndex; }
            set { selectedPhraseIndex = value; RaisePropertyChanged(); }
        }

        private string translation;
        public string Translation
        {
            get { return translation; }
            set 
            { 
                translation = value;
                RaisePropertyChanged();
            }
        }
        
        private string phrase;
        public string Phrase
        {
            get { return phrase; }
            set 
            { 
                phrase = value;
                this.RaisePropertyChanged();
            }
        }

        private bool isWordExists;
        public bool IsWordExists
        {
            get { return isWordExists; }
            set 
            { 
                isWordExists = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SaveCmd { get; protected set; }
        private void Save(object parameter)
        {
            this.IsWordExists = false;
            if (!this.wordStorage.IsWordAlreadyExists(this.Word))
            {
                this.Word.SavePointer.DeleteSavePoint();
                if (isNewWord)
                {
                    this.DictionaryEditorVM.Words.Add(this.Word);
                    this.wordStorage.Add(this.Word);
                }
                else
                    this.wordStorage.Update(this.Word);
                this.RaiseOnClosed();
            }
            else
                this.IsWordExists = true;
        }

        public ICommand CancelCmd { get; protected set; }
        private void Cancel(object parameter)
        {
            this.Word.SavePointer.RollbackToSavePoint();
            this.RaiseOnClosed();
        }

        public ICommand SaveTranslationCmd { get; protected set; }
        private void SaveTranslation(object parameter)
        {
            if (this.SelectedTranslationIndex >= 0)
            {
                this.Word.Translations[this.SelectedTranslationIndex].Spelling = this.Translation;
                this.FullRefresh();
            }
        }

        public ICommand AddTranslationCmd { get; protected set; }
        private void AddTranslation(object parameter)
        {
            if (!String.IsNullOrEmpty(this.Translation))
            {
                this.Word.Translations.Add(new Word() { ID = Guid.NewGuid(), Language = Language.Rus, Spelling = this.Translation });
                this.FullRefresh();
            }
        }

        public ICommand DeleteTranslationCmd { get; protected set; }
        private void DeleteTranslation(object parameter)
        {
            if (this.SelectedTranslationIndex >= 0)
            {
                var index = this.selectedTranslationIndex;
                this.Word.Translations.RemoveAt(this.SelectedTranslationIndex);
                this.FullRefresh();
                
                if (index == this.Word.Translations.Count)
                    this.SelectedTranslationIndex = index - 1;
                else if (this.Word.Translations.Any())
                    this.SelectedTranslationIndex = index;

            }
        }

        public ICommand SavePhraseCmd { get; protected set; }
        private void SavePhrase(object parameter)
        {
            if (this.SelectedPhraseIndex >= 0)
            {
                this.Word.Phrases[this.SelectedPhraseIndex] = this.Phrase;
                this.FullRefresh();
            }
        }

        public ICommand AddPhraseCmd { get; protected set; }
        private void AddPhrase(object parameter)
        {
            if (!String.IsNullOrEmpty(this.Phrase))
            {
                this.Word.Phrases.Add(this.Phrase);
                this.FullRefresh();
            }
        }

        public ICommand DeletePhraseCmd { get; protected set; }
        private void DeletePhrase(object parameter)
        {
            if (this.SelectedPhraseIndex >= 0)
            {
                this.Word.Phrases.RemoveAt(this.SelectedPhraseIndex);
                this.FullRefresh();
            }
        }

        public ICommand MoveTranslationUpCmd { get; set; }
        private void MoveTranslationUp(object parameter)
        {
            if (this.Word.Translations.Count > 1 && this.selectedTranslationIndex > 0)
            {
                var source = this.Word.Translations[this.selectedTranslationIndex];
                this.Word.Translations[this.selectedTranslationIndex] = this.Word.Translations[this.selectedTranslationIndex - 1];
                this.Word.Translations[this.selectedTranslationIndex - 1] = source;

                var index = this.SelectedTranslationIndex - 1;
                this.FullRefresh();
                this.SelectedTranslationIndex = index;
            }
        }

        public ICommand MoveTranslationDownCmd { get; set; }
        private void MoveTranslationDown(object parameter)
        {
            if (this.Word.Translations.Count > 1 && this.selectedTranslationIndex < this.Word.Translations.Count - 1)
            {
                var source = this.Word.Translations[this.selectedTranslationIndex];
                this.Word.Translations[this.selectedTranslationIndex] = this.Word.Translations[this.selectedTranslationIndex + 1];
                this.Word.Translations[this.selectedTranslationIndex + 1] = source;

                var index = this.SelectedTranslationIndex + 1;
                this.FullRefresh();
                this.SelectedTranslationIndex = index;
            }
        }

        public event EventHandler OnClosed;
        private void RaiseOnClosed()
        {
            if (OnClosed != null)
                OnClosed(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FullRefresh()
        {
            var temp = this.Word;
            this.Word = null;
            this.Word = temp;
        }
    }
}
