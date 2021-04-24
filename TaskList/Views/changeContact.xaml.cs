using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for changeContact.xaml
    /// </summary>
    public partial class changeContact : Window
    {
        public changeContact()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void DatePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //Filtering only numbers input
            Regex regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void nameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(String.IsNullOrWhiteSpace(nameTB.Text) || String.IsNullOrWhiteSpace(numberTB.Text))
            {
                changeBtn.IsEnabled = false;
            }
            else
            {
                changeBtn.IsEnabled = true;
            }
        }
    }
}
