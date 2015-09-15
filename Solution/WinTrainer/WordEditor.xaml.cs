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
    /// Interaction logic for WordEditor.xaml
    /// </summary>
    public partial class WordEditor : MetroWindow
    {
        public WordEditorVM ViewModel { get; protected set; }

        public WordEditor(WordEditorVM viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = viewModel;
            InitializeComponent();

            this.ViewModel.OnClosed += ViewModel_OnClosed;
        }

        void ViewModel_OnClosed(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
