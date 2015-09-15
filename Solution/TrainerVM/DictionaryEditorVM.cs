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
    public class DictionaryEditorVM//: INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        public MainVM MainVM { get; protected set; }

        public DictionaryEditorVM(MainVM mainVM, Word word = null)
        {
            this.MainVM = mainVM;

            this.SaveCmd = new Command(Save);
            this.CancelCmd = new Command(Cancel);
            this.AddWordCmd = new Command(AddWord);
            this.EditWordCmd = new Command(EditWord);
            this.DeleteWordCmd = new Command(DeleteWord);
            
            this.MainVM.WordStorage.Reopen();
            this.Words = new ObservableCollection<Word>(this.MainVM.WordStorage.GetWordsByLanguage(Language.En));

            if (word != null)
                this.GoToWord(word);
        }

        private void GoToWord(Word word)
        {
            var wordToSelect = this.Words.FirstOrDefault(w => w.ID == word.ID);
            if (wordToSelect != null)
                this.SelectedWord = wordToSelect;
        }

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
            this.MainVM.WordStorage.Save();
            this.RaiseOnClose();
        }

        public ICommand CancelCmd { get; protected set; }
        private void Cancel(object parameter)
        {
            this.MainVM.WordStorage.Reopen();
            this.RaiseOnClose();
        }

        public event EventHandler OnClose;
        public void RaiseOnClose()
        {
            if (OnClose != null)
                OnClose(this, EventArgs.Empty);
        }

        public event EventHandler<MainVM.ViewModelArgs> OnOpenEditor;
        public void RaiseOnOpenEditor(WordEditorVM editorVM)
        {
            if (OnOpenEditor != null)
                OnOpenEditor(this, new MainVM.ViewModelArgs() { ViewModel = editorVM } );
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
            this.MainVM.WordStorage.AllList.Remove(SelectedWord);
            this.Words.Remove(SelectedWord);
        }
    }
}
