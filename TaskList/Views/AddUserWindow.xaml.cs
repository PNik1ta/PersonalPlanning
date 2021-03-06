﻿using System;
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
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
            addBtn.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void nameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableButton();
        }

        private void passTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckEnableButton();

        }

        //Checking if add button is enabled
        private void CheckEnableButton()
        {
            if (String.IsNullOrWhiteSpace(nameTB.Text) || String.IsNullOrWhiteSpace(passTB.Text))
            {
                addBtn.IsEnabled = false;
            }
            else
            {
                addBtn.IsEnabled = true;
            }
        }
    }
}
