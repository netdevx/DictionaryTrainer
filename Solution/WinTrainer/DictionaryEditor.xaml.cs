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
using System.Windows.Shapes;

using MahApps.Metro.Controls;

using AnSoft.DictionaryTrainer.ViewModel;

namespace AnSoft.DictionaryTrainer.WinApp
{
    /// <summary>
    /// Interaction logic for DictionatyEditor.xaml
    /// </summary>
    public partial class DictionaryEditor : MetroWindow
    {
        public DictionaryEditorVM ViewModel { get; protected set; }
        
        public DictionaryEditor(object viewModel)
        {
            this.DataContext = viewModel;
            this.ViewModel = viewModel as DictionaryEditorVM;
            
            InitializeComponent();

            this.ViewModel.OnOpenEditor += ViewModel_OnOpenEditor;
            this.ViewModel.OnClose += ViewModel_OnClose;
        }

        void ViewModel_OnClose(object sender, EventArgs e)
        {
            this.Close();
        }

        void ViewModel_OnOpenEditor(object sender, ViewModelArgs e)
        {
            var wordEditor = new WordEditor(e.ViewModel as WordEditorVM);
            wordEditor.ShowDialog();
        }
    }
}
