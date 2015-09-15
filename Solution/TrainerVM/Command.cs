using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AnSoft.DictionaryTrainer.ViewModel
{
    public class Command: ICommand
    {
        public Command(Action<object> executeDelegate)
        {
            this.ExecuteDelegate = executeDelegate;
        }

        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        
        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (this.ExecuteDelegate != null)
                this.ExecuteDelegate(parameter);
        }
    }
}
