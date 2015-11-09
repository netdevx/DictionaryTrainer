using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class DictionaryEditorVM
    {
        public DictionaryEditorVM(Word word = null)
        {
            this.WordStorage = DIContainer.Instance.Get<IWordStorage>();

            this.SaveCmd = new Command(Save);
            this.CancelCmd = new Command(Cancel);
            this.AddWordCmd = new Command(AddWord);
            this.EditWordCmd = new Command(EditWord);
            this.DeleteWordCmd = new Command(DeleteWord);
            
            this.WordStorage.Reopen();
            this.Words = new ObservableCollection<Word>(this.WordStorage.GetWordsByLanguage(MainVM.Instance.Language));

            if (word != null)
                this.GoToWord(word);
        }

        private IWordStorage WordStorage;

        public ObservableCollection<Word> Words { get; protected set; }

        private Word selectedWord;
        public Word SelectedWord
        {
            get { return selectedWord; }
            set 
            { 
                selectedWord = value;
                //this.RaisePropertyChanged("SelectedWord");
            }
        }

        public ICommand SaveCmd { get; protected set; }
        private void Save(object parameter)
        {
            this.WordStorage.Save();
            this.RaiseOnClose();
        }

        public ICommand CancelCmd { get; protected set; }
        private void Cancel(object parameter)
        {
            this.WordStorage.Reopen();
            this.RaiseOnClose();
        }

        public ICommand AddWordCmd { get; protected set; }
        private void AddWord(object parameter)
        {
            var wordEditor = new WordEditorVM(this, null);
            this.RaiseOnOpenEditor(wordEditor);
        }

        public ICommand EditWordCmd { get; protected set; }
        private void EditWord(object parameter)
        {
            var wordEditor = new WordEditorVM(this, this.SelectedWord);
            this.RaiseOnOpenEditor(wordEditor);
        }

        public ICommand DeleteWordCmd { get; protected set; }
        private void DeleteWord(object parameter)
        {
            DIContainer.Instance.Get<MainVM>().WordStorage.Delete(SelectedWord);
            this.Words.Remove(SelectedWord);
        }

        public event EventHandler OnClose;
        public void RaiseOnClose()
        {
            if (OnClose != null)
                OnClose(this, EventArgs.Empty);
        }

        public event EventHandler<ViewModelArgs> OnOpenEditor;
        public void RaiseOnOpenEditor(WordEditorVM editorVM)
        {
            if (OnOpenEditor != null)
                OnOpenEditor(this, new ViewModelArgs() { ViewModel = editorVM });
        }

        private void GoToWord(Word word)
        {
            var wordToSelect = this.Words.FirstOrDefault(w => w.ID == word.ID);
            if (wordToSelect != null)
                this.SelectedWord = wordToSelect;
        }
    }
}
