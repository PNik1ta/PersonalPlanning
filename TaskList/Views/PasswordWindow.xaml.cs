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
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
            enterBtn.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Application.Current.Shutdown();
        }

        private void passBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(passBox.Password))
            {
                enterBtn.IsEnabled = false;
            }
            else
            {
                enterBtn.IsEnabled = true;
            }
        }
    }
}
