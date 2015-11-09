using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AnSoft.DictionaryTrainer.Model;
using Ninject;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class MainVM
    {
        public MainVM(IWordStorage wordStorage, IStorage<WordResult> wordResultStorage, TrainerVM trainerVM)
        {
            this.WordStorage = wordStorage;
            this.WordResultStorage = wordResultStorage;

            this.TrainerVM = trainerVM;

            this.Exit = new Command((o) => 
            {
                if (this.OnExitApp != null)
                    this.OnExitApp(this, EventArgs.Empty);
            });

            this.OpenEditorCmd = new Command(this.OpenEditor);
        }

        public IWordStorage WordStorage { get; protected set; }
        public IStorage<WordResult> WordResultStorage { get; protected set; }

        public TrainerVM TrainerVM { get; protected set; }

        public ICommand Exit { get; protected set; }

        public ICommand OpenEditorCmd { get; protected set; }
        private void OpenEditor(object word)
        {
            var vm = new DictionaryEditorVM(word as Word);
            
            if (this.OnOpenEditor != null)
                this.OnOpenEditor(this, new ViewModelArgs() { ViewModel = vm });
        }

        public event EventHandler OnExitApp;

        public event EventHandler<ViewModelArgs> OnOpenEditor;
    }
}
