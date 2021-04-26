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

namespace TaskList.Views
{
    /// <summary>
    /// Interaction logic for AddPhraseWindow.xaml
    /// </summary>
    public partial class AddPhraseWindow : Window
    {
        public AddPhraseWindow()
        {
            InitializeComponent();
            addPhraseBtn.IsEnabled = false;
        }

        private void authorTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableButton();
        }

        private void phraseTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableButton();
        }

        //Checking if add button is enabled
        private void CheckEnableButton()
        {
            if (String.IsNullOrWhiteSpace(authorTB.Text) || String.IsNullOrWhiteSpace(phraseTB.Text))
            {
                addPhraseBtn.IsEnabled = false;
            }
            else
            {
                addPhraseBtn.IsEnabled = true;
            }
        }
    }
}
