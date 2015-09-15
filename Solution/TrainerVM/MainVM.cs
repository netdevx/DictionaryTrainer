using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using AnSoft.DictionaryTrainer.Model;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class MainVM
    {
        public IWordStorage WordStorage { get; protected set; }
        
        public IStorage<WordResult> WordResultStorage { get; protected set; }

        public TrainerVM TrainerVM { get; protected set; }

        public MainVM()
        {
            this.WordStorage = new WordStorage("c:\\tmp\\dictionary.dat");
            this.WordResultStorage = new WordResultStorage("results.dat");

            this.TrainerVM = new TrainerVM(this);

            this.Exit = new Command((o) => 
            {
                if (this.OnExitApp != null)
                    this.OnExitApp(this, EventArgs.Empty);
            });

            this.OpenEditorCmd = new Command(this.OpenEditor);
        }

        public event EventHandler OnExitApp;

        public event EventHandler<ViewModelArgs> OnOpenEditor;

        public ICommand Exit { get; protected set; }

        public ICommand OpenEditorCmd { get; protected set; }
        private void OpenEditor(object word)
        {
            var vm = new DictionaryEditorVM(this, word as Word);
            
            if (this.OnOpenEditor != null)
                this.OnOpenEditor(this, new ViewModelArgs() { ViewModel = vm });
        }

        public class ViewModelArgs: EventArgs
        {
            public object ViewModel { get; set; }
        }
    }
}
