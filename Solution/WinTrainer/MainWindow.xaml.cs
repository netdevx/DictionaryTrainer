using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;

using AnSoft.DictionaryTrainer.ViewModel;

namespace AnSoft.DictionaryTrainer.WinApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainVM MainVM { get; protected set; }

        public MainWindow()
        {
            InitializeComponent();

            this.MainVM = DIContainer.Instance.Get<MainVM>();
            this.MainVM.OnExitApp += ExitAppHandler;
            this.MainVM.OnOpenEditor += ShowEditorHandler;
            
            this.DataContext = this.MainVM;
        }

        private void ExitAppHandler(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ShowEditorHandler(object sender, ViewModelArgs e)
        {
            var editor = new DictionaryEditor(e.ViewModel);
            editor.ShowDialog();
        }

        private void Control_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (sender as Control).Focus();
        }
    }
}
