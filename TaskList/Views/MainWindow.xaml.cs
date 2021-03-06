﻿using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Newtonsoft.Json;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TaskList.Models;
using TaskList.Presenters;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace TaskList.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        DispatcherTimer timer = new DispatcherTimer();
        private Person person = new Person();
        private bool isMenuOpen = false;
        private AppViewPresenter presenter = new AppViewPresenter();
        private AddTaskWindow addTaskWindow = new AddTaskWindow();
        private AddUserWindow addUserWindow = new AddUserWindow();
        private PasswordWindow passwordWindow = new PasswordWindow();
        private ChangeTaskWindow changeTaskWindow = new ChangeTaskWindow();
        private AddPhraseWindow phraseWindow = new AddPhraseWindow();
        private addContactWindow contactWindow = new addContactWindow();
        private changeContact changeContact = new changeContact();
        public TaskbarIcon MyTaskbarIcon = new TaskbarIcon();
        public string connectionString;
        public DataTable phraseTable;
        Currency currencies;
        public MainWindow()
        {
            InitializeComponent();
            presenter.view = this;
            person = presenter.ReadPerson();
            MyTaskbarIcon.Visibility = Visibility.Visible;
            MyTaskbarIcon.ToolTipText = "Task List";
            MyTaskbarIcon.MenuActivation = PopupActivationMode.LeftOrRightClick;
            MyTaskbarIcon.PopupActivation = PopupActivationMode.DoubleClick;
            MyTaskbarIcon.Icon = new System.Drawing.Icon(@"..\..\..\Images\toDoImg.ico");
            MyTaskbarIcon.TrayLeftMouseUp += iconOnClick;
            ContextMenu contextMenu = this.FindResource("cmButton") as ContextMenu;
            MyTaskbarIcon.ContextMenu = contextMenu;
            Popup pu = new Popup();
            pu.Child = MyTaskbarIcon;



            passwordWindow.enterBtn.Click += CheckPass;
            passwordWindow.Closing += PassWindowClosing;
            passwordWindow.AddUserBtn.Click += addUserInPassWindow;
            passwordWindow.ShowDialog();


            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            RadialGradientBrush gradientBrush = new RadialGradientBrush();
            gradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 1));
            this.Background = gradientBrush;
            addTaskWindow.Background = gradientBrush;
            addUserWindow.Background = gradientBrush;
            changeTaskWindow.Background = gradientBrush;
            phraseWindow.Background = gradientBrush;
            passwordWindow.Background = gradientBrush;
            menuColumn.Background = gradientBrush;
            changeTheme.SelectedItem = "Light";
            printPersonInfo();
            UpdateTasks();

            presenter.ShowWeather();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Notify;
            timer.Start();

            avatarImg.ImageSource = new BitmapImage(new Uri(person.Avatar, UriKind.RelativeOrAbsolute)); ;

            passwordWindow.AddUserBtn.Click += addUserBtn_Click;

            addTaskWindow.addBtn.Click += addBtn_Click;
            addUserWindow.addBtn.Click += addUser_Click;
            phraseWindow.addPhraseBtn.Click += addData;
            changeContact.changeBtn.Click += changeContactEvent;
            changeTaskWindow.changeBtn.Click += changeTask_Click;
            contactWindow.addBtn.Click += addContactEvent;

            changeTaskWindow.levelCB.Items.Add("Low");
            changeTaskWindow.levelCB.Items.Add("Normal");
            changeTaskWindow.levelCB.Items.Add("High");
            changeTaskWindow.levelCB.Items.Add("Important");
            changeTheme.Items.Add("Light");
            changeTheme.Items.Add("Dark");

            calculateCostBtn.IsEnabled = false;
        }

        private void iconOnClick(object sender, RoutedEventArgs e)
        {
            this.Show();
        }


        public void UpdateContactListBox()
        {
            contactLB.Items.Clear();
            foreach (var contact in person.Contacts)
            {
                contactLB.Items.Add(contact.ToString());
            }
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            presenter.HamburgerMenu();
        }

        private void personBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowPerson();
        }

        private void toDoInfo_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowTasks();
        }

        private void achievmentsBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowAchievments();
        }

        private void motivationBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowMotivation();
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowSettings();
        }

        private void changeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeUser();
            presenter.RestartApp();
        }

        private void printPersonInfo()
        {
            presenter.PrintPersonInfo();

        }

        private void removeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveUser();
        }

        private void addTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddTask();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddTaskEvent();
        }

        private void removeTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveTask();

        }
        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddUser();

        }
        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddUserEvent();
        }
        private void CheckPass(object sender, RoutedEventArgs e)
        {
            presenter.CheckPass();
        }
        private void PassWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            presenter.PassWindowClosing();

        }
        private void changeTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeTask();
        }

        private void changeTask_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeTaskEvent();
        }

        private void completedTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.CompleteTask();

        }

        private void levelPB_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            presenter.XPValueChanging();
        }

        private void changeThemeBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeTheme();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            presenter.UpdateDB();
        }

        private void updateDataBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.UpdateDB();
        }

        private void moneyExchangeBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowMoneyExchange();
        }

        private void removeDataBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveData();
        }

        private void addDataBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddData();
        }

        private void addData(object sender, RoutedEventArgs e)
        {
            presenter.AddDataEvent();
        }

        private void diagramBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowDiagram();
        }

        private void calculateCostBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.CalculateCost();
        }

        private void Notify(object sender, EventArgs e)
        {
            presenter.Notify();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.Exit();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void addContactBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddContact();
        }

        private void addContactEvent(object sender, RoutedEventArgs e)
        {
            presenter.AddContactEvent();

        }

        private void removeContactBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveContact();
        }

        private void updateContactBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeContact();
        }

        private void changeContactEvent(object sender, RoutedEventArgs e)
        {
            presenter.ChangeContactEvent();
        }

        private void contactBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowContacts();
        }

        private void noteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowNotes();
        }

        private void addNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddNote();
        }

        private void removeNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveNote();
        }

        private void changeNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeNote();
        }

        private void saveNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.SaveNote();
        }

        private void loadNoteBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.LoadNote();
        }

        public void UpdateProgress()
        {
            levelPB.Value = person.XP;
            levelLabel_2.Content = person.Level;
            countOfComplitedTasksLabel_2.Content = person.CountOfComplitedTasks;
        }

        public void UpdateDB()
        {
            motivationLB.Items.Clear();
            foreach (var phrase in presenter.db.Phrases)
            {
                motivationLB.Items.Add(phrase.ToString());
            }
        }

        public void UpdateUsers()
        {
            peopleLB.Items.Clear();
            People people = new People();
            foreach (var p in people.people)
            {
                peopleLB.Items.Add(p.Name);
            }
        }

        public void HideAll()
        {
            personSP_1.Visibility = Visibility.Hidden;
            personSP_2.Visibility = Visibility.Hidden;

            addTaskBtn.Visibility = Visibility.Hidden;
            removeTaskBtn.Visibility = Visibility.Hidden;
            changeTaskBtn.Visibility = Visibility.Hidden;
            completedTaskBtn.Visibility = Visibility.Hidden;
            tasksLB.Visibility = Visibility.Hidden;

            settingsGrid.Visibility = Visibility.Hidden;

            leveLabel_1.Visibility = Visibility.Hidden;
            levelLabel_2.Visibility = Visibility.Hidden;
            levelPB.Visibility = Visibility.Hidden;
            countOfComplitedTasksLabel_1.Visibility = Visibility.Hidden;
            countOfComplitedTasksLabel_2.Visibility = Visibility.Hidden;

            motivationGrid.Visibility = Visibility.Hidden;

            salaryLabel.Visibility = Visibility.Hidden;
            foodCostLabel.Visibility = Visibility.Hidden;
            utilitiesCostLabel.Visibility = Visibility.Hidden;
            personalCostLabel.Visibility = Visibility.Hidden;
            salaryTB.Visibility = Visibility.Hidden;
            foodCostTB.Visibility = Visibility.Hidden;
            utilitiesCostTB.Visibility = Visibility.Hidden;
            personalCostTB.Visibility = Visibility.Hidden;
            foodPercentCostLabel.Visibility = Visibility.Hidden;
            foodPercentCostLabel_2.Visibility = Visibility.Hidden;
            utilitiesPercentCostLabel.Visibility = Visibility.Hidden;
            utilitiesPercentCostLabel_2.Visibility = Visibility.Hidden;
            personalPercentCostLabel.Visibility = Visibility.Hidden;
            personalPercentCostLabel_2.Visibility = Visibility.Hidden;
            calculateCostBtn.Visibility = Visibility.Hidden;
            leftMoneyPercentLabel.Visibility = Visibility.Hidden;
            leftMoneyPercentLabel_2.Visibility = Visibility.Hidden;

            chartGrid.Visibility = Visibility.Hidden;

            contactGrid.Visibility = Visibility.Hidden;

            notesGrid.Visibility = Visibility.Hidden;

            moneyExchangeGrid.Visibility = Visibility.Hidden;

            weatherGrid.Visibility = Visibility.Hidden;

            xpCount.Visibility = Visibility.Hidden;
            Xplabel.Visibility = Visibility.Hidden;

            personAvatarSP.Visibility = Visibility.Hidden;
        }

        public void UpdateTasks()
        {
            tasksLB.Items.Clear();
            for (int i = 0; i < person.ToDos.Count; i++)
            {
                if (person.ToDos[i].Level == Level.Low)
                {
                    tasksLB.Items.Add(new ListViewItem { Content = person.ToDos[i].ToString(), Background = System.Windows.Media.Brushes.LightBlue });
                }

                else if (person.ToDos[i].Level == Level.Normal)
                {
                    tasksLB.Items.Add(new ListViewItem { Content = person.ToDos[i].ToString(), Background = System.Windows.Media.Brushes.LightGreen });
                }

                else if (person.ToDos[i].Level == Level.High)
                {
                    tasksLB.Items.Add(new ListViewItem { Content = person.ToDos[i].ToString(), Background = System.Windows.Media.Brushes.Orange });
                }

                else
                {
                    tasksLB.Items.Add(new ListViewItem { Content = person.ToDos[i].ToString(), Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 112, 112)), Foreground = System.Windows.Media.Brushes.White });

                }
            }
        }

        public void UpdateNotesList()
        {
            notesLB.Items.Clear();
            foreach (var note in person.Notes)
            {
                notesLB.Items.Add(note.Title);
            }
        }

        public void AddTaskEvent()
        {
            bool isAdd = true;

            foreach (var task in person.ToDos)
            {
                if (addTaskWindow.titleTB.Text == task.Name)
                {
                    isAdd = false;
                    break;
                }
            }

            if (!isAdd)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You can't add task with the same title";
                window.ShowDialog();
            }
            else
            {
                if (addTaskWindow.levelCB.SelectedItem == null)
                {
                    presenter.AddTask(addTaskWindow.titleTB.Text, addTaskWindow.commentTB.Text, addTaskWindow.dateDP.Value, Level.Low, addTaskWindow.notifyCB.IsChecked, person);
                }

                else
                {
                    if (addTaskWindow.levelCB.SelectedItem.ToString() == "Low")
                    {
                        presenter.AddTask(addTaskWindow.titleTB.Text, addTaskWindow.commentTB.Text, addTaskWindow.dateDP.Value, Level.Low, addTaskWindow.notifyCB.IsChecked, person);
                    }

                    else if (addTaskWindow.levelCB.SelectedItem.ToString() == "Normal")
                    {
                        presenter.AddTask(addTaskWindow.titleTB.Text, addTaskWindow.commentTB.Text, addTaskWindow.dateDP.Value, Level.Normal, addTaskWindow.notifyCB.IsChecked, person);
                    }

                    else if (addTaskWindow.levelCB.SelectedItem.ToString() == "High")
                    {
                        presenter.AddTask(addTaskWindow.titleTB.Text, addTaskWindow.commentTB.Text, addTaskWindow.dateDP.Value, Level.High, addTaskWindow.notifyCB.IsChecked, person);
                    }

                    else if (addTaskWindow.levelCB.SelectedItem.ToString() == "Important")
                    {
                        presenter.AddTask(addTaskWindow.titleTB.Text, addTaskWindow.commentTB.Text, addTaskWindow.dateDP.Value, Level.Important, addTaskWindow.notifyCB.IsChecked, person);
                    }
                }

                presenter.SavePerson(person);
                UpdateTasks();
                addTaskWindow.Close();
            }
        }

        public void ChangeUser()
        {
            if (peopleLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose the user";
                window.ShowDialog();
            }
            else
            {
                People people = new People();
                foreach (var p in people.people)
                {
                    if (peopleLB.SelectedItem.ToString() == p.Name)
                    {
                        person = p;
                    }
                }


                presenter.SavePerson(person);
                Application.Current.Shutdown();
            }
        }

        public void RemoveUser()
        {
            if (peopleLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose the user";
                window.ShowDialog();
            }
            else
            {

                People people = new People();
                for (int i = 0; i < people.people.Count; i++)
                {
                    if (peopleLB.SelectedItem.ToString() == people.people[i].Name)
                    {
                        if (passBox.Password == people.people[i].Password)
                        {
                            presenter.RemoveUser(i);
                        }
                        else
                        {
                            dialogWindow window = new dialogWindow();
                            window.dialogMessage.Text = "User password is incorrect. You must enter user's password you want to delete";
                            window.ShowDialog();
                        }
                    }
                }
            }

            UpdateUsers();
        }

        public void RemoveTask()
        {
            if (tasksLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose the task";
                window.ShowDialog();
            }
            else
            {
                for (int i = 0; i < person.ToDos.Count; i++)
                {
                    if (tasksLB.SelectedItem.ToString().Contains(person.ToDos[i].ToString()))
                    {
                        presenter.RemoveTask(i, person);
                    }
                }

                presenter.SavePerson(person);
                UpdateTasks();
            }
        }

        public void AddUserEvent()
        {
            People people = new People();

            bool isAdd = true;
            foreach (var p in people.people)
            {
                if (addUserWindow.nameTB.Text == p.Name)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "You can't add users with the same name";
                    window.ShowDialog();
                    isAdd = false;
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(addUserWindow.nameTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Name field is empty";
                window.ShowDialog();
            }

            else if (String.IsNullOrWhiteSpace(addUserWindow.surnameTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Surname field is empty";
                window.ShowDialog();
            }

            else if (String.IsNullOrWhiteSpace(addUserWindow.ageTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Age field is empty";
                window.ShowDialog();
            }

            else if (String.IsNullOrWhiteSpace(addUserWindow.passTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Pass field is empty";
                window.ShowDialog();
            }

            if (isAdd)
            {
                int age;
                if (!int.TryParse(addUserWindow.ageTB.Text, out age))
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Age is not a number";
                    window.ShowDialog();
                    age = 0;
                }
                else
                {
                    person.Age = age;
                }
                presenter.AddUser(addUserWindow.nameTB.Text, addUserWindow.surnameTB.Text, age, addUserWindow.passTB.Text);
                addUserWindow.Close();
            }
        }

        public void ChangeTask()
        {
            if (tasksLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose the task";
                window.ShowDialog();
            }
            else
            {
                for (int i = 0; i < person.ToDos.Count; i++)
                {
                    if (tasksLB.SelectedItem.ToString().Contains(person.ToDos[i].ToString()))
                    {
                        changeTaskWindow.titleTB.Text = person.ToDos[i].Name;
                        changeTaskWindow.dateDP.SelectedDate = person.ToDos[i].Date;
                        changeTaskWindow.commentTB.Text = person.ToDos[i].Comment;
                        if (person.ToDos[i].Level == Level.Low)
                        {
                            changeTaskWindow.levelCB.SelectedItem = "Low";
                        }

                        else if (person.ToDos[i].Level == Level.Normal)
                        {
                            changeTaskWindow.levelCB.SelectedItem = "Normal";
                        }

                        else if (person.ToDos[i].Level == Level.High)
                        {
                            changeTaskWindow.levelCB.SelectedItem = "High";
                        }

                        else if (person.ToDos[i].Level == Level.Important)
                        {
                            changeTaskWindow.levelCB.SelectedItem = "Important";
                        }


                        changeTaskWindow.ShowDialog();
                        UpdateTasks();
                        break;
                    }
                }
            }
        }

        public void ChangeTaskEvent()
        {
            for (int i = 0; i < person.ToDos.Count; i++)
            {
                if (tasksLB.SelectedItem.ToString().Contains(person.ToDos[i].ToString()))
                {
                    if (changeTaskWindow.levelCB.SelectedItem.ToString() == "Low")
                    {
                        presenter.ChangeTask(person, i, changeTaskWindow.titleTB.Text, changeTaskWindow.commentTB.Text, changeTaskWindow.dateDP.SelectedDate, Level.Low);
                    }

                    else if (changeTaskWindow.levelCB.SelectedItem.ToString() == "Normal")
                    {
                        presenter.ChangeTask(person, i, changeTaskWindow.titleTB.Text, changeTaskWindow.commentTB.Text, changeTaskWindow.dateDP.SelectedDate, Level.Normal);
                    }

                    else if (changeTaskWindow.levelCB.SelectedItem.ToString() == "High")
                    {
                        presenter.ChangeTask(person, i, changeTaskWindow.titleTB.Text, changeTaskWindow.commentTB.Text, changeTaskWindow.dateDP.SelectedDate, Level.High);
                    }

                    else if (changeTaskWindow.levelCB.SelectedItem.ToString() == "Important")
                    {
                        presenter.ChangeTask(person, i, changeTaskWindow.titleTB.Text, changeTaskWindow.commentTB.Text, changeTaskWindow.dateDP.SelectedDate, Level.Important);
                    }
                }
            }
            presenter.SavePerson(person);
            changeTaskWindow.Close();
        }

        public void CompleteTask()
        {
            if (tasksLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose the task";
                window.ShowDialog();
            }
            else
            {
                for (int i = 0; i < person.ToDos.Count; i++)
                {
                    if ((tasksLB.SelectedItem as ListViewItem).Content.ToString() == person.ToDos[i].ToString())
                    {
                        presenter.RemoveTask(i, person);
                        presenter.CompleteTask(person);
                        UpdateTasks();
                        UpdateProgress();
                        presenter.SavePerson(person);
                        break;
                    }
                }


            }
        }

        public void RemoveData()
        {
            presenter.RemoveData(motivationLB.SelectedItem.ToString());
            UpdateDB();
        }

        public void AddDataEvent()
        {
            presenter.AddData(phraseWindow.authorTB.Text, phraseWindow.phraseTB.Text);
            UpdateDB();
            phraseWindow.Close();
        }

        public void CalculateCost()
        {
            if (salaryTB.Text == string.Empty || foodCostTB.Text == string.Empty
               || utilitiesCostTB.Text == string.Empty || personalCostTB.Text == string.Empty)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "This fields can't be empty";
                window.ShowDialog();
            }
            else
            {
                long salary = 0;
                long foodCost = 0;
                long utilityCost = 0;
                long personalCost = 0;
                long leftMoney = 0;


                long.TryParse(salaryTB.Text, out salary);
                long.TryParse(foodCostTB.Text, out foodCost);
                long.TryParse(utilitiesCostTB.Text, out utilityCost);
                long.TryParse(personalCostTB.Text, out personalCost);

                if (foodCost > salary)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Food costs can't be more than salary";
                    window.ShowDialog();
                }

                else if (utilityCost > salary)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Utility cost can't be more than salary";
                    window.ShowDialog();
                }

                else if (personalCost > salary)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Personal costs can't be more than salary";
                    window.ShowDialog();
                }

                else if (foodCost < 0 || utilityCost < 0 || personalCost < 0)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Cost can't be less than 0";
                    window.ShowDialog();
                }

                else if (salary < (foodCost + utilityCost + personalCost))
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Your salary is less tha your costs";
                    window.ShowDialog();
                }

                else
                {
                    leftMoney = salary - foodCost - utilityCost - personalCost;

                    long foodPercent = foodCost * 100 / salary;
                    long utilityPercent = utilityCost * 100 / salary;
                    long personalPercent = personalCost * 100 / salary;
                    long leftPercent = leftMoney * 100 / salary;

                    foodPercentCostLabel_2.Content = foodPercent + "%";
                    utilitiesPercentCostLabel_2.Content = utilityPercent + "%";
                    personalPercentCostLabel_2.Content = personalPercent + "%";
                    leftMoneyPercentLabel_2.Content = leftPercent + "%";

                    ((PieSeries)mcChart.Series[0]).ItemsSource = new KeyValuePair<string, long>[] {
            new KeyValuePair<string, long>("Food Percent", foodPercent),
            new KeyValuePair<string, long>("Utility Percent", utilityPercent),
            new KeyValuePair<string, long>("Personal Percent", personalPercent),
            new KeyValuePair<string, long>("Left", leftPercent)
    };
                }
            }
        }

        public void Notify()
        {
            foreach (var toDo in person.ToDos)
            {
                if (toDo.isNotify == true)
                {
                    if (DateTime.Now.Date == toDo.Date.Value.Date &&
                        DateTime.Now.Hour == toDo.Date.Value.Hour &&
                        DateTime.Now.Minute == toDo.Date.Value.Minute &&
                        DateTime.Now.Second == toDo.Date.Value.Second)
                    {
                        //dialogWindow window = new dialogWindow();
                        //window.dialogMessage.Text = toDo.Name + ": " + toDo.Comment;
                        //window.ShowDialog();

                        var notificationManager = new NotificationManager();
                        if (toDo.Level == Level.Low)
                        {
                            notificationManager.Show(new NotificationContent
                            {
                                Title = toDo.Name,
                                Message = toDo.Comment,
                                Type = NotificationType.Information
                            });
                        }

                        else if (toDo.Level == Level.Normal)
                        {
                            notificationManager.Show(new NotificationContent
                            {
                                Title = toDo.Name,
                                Message = toDo.Comment,
                                Type = NotificationType.Success
                            });
                        }

                        else if (toDo.Level == Level.High)
                        {
                            notificationManager.Show(new NotificationContent
                            {
                                Title = toDo.Name,
                                Message = toDo.Comment,
                                Type = NotificationType.Warning
                            });
                        }

                        else
                        {
                            notificationManager.Show(new NotificationContent
                            {
                                Title = toDo.Name,
                                Message = toDo.Comment,
                                Type = NotificationType.Warning
                            });
                        }

                    }
                }
            }
        }

        public void AddContactEvent()
        {
            int phone = 0;
            if (int.TryParse(contactWindow.numberTB.Text, out phone))
            {
                presenter.AddContact(contactWindow.nameTB.Text, contactWindow.surnameTB.Text, phone, person);

            }
            else
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Phone number must include only numbers";
                window.ShowDialog();
            }
            contactWindow.Hide();

            presenter.SavePerson(person);
            person = presenter.ReadPerson();
            UpdateContactListBox();
        }

        public void RemoveContact()
        {
            if (contactLB.SelectedItem != null)
            {
                for (int i = 0; i < person.Contacts.Count; i++)
                {
                    if (contactLB.SelectedItem.ToString().Contains(person.Contacts[i].ToString()))
                    {
                        presenter.RemoveContact(i, person);
                    }
                }
            }

            else
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose a contact";
                window.ShowDialog();
            }

            presenter.SavePerson(person);
            person = presenter.ReadPerson();
            UpdateContactListBox();
        }

        public void ChangeContactEvent()
        {
            if (contactLB.SelectedItem != null)
            {
                for (int i = 0; i < person.Contacts.Count; i++)
                {
                    if (contactLB.SelectedItem.ToString().Contains(person.Contacts[i].ToString()))
                    {
                        int number;
                        if (int.TryParse(changeContact.numberTB.Text, out number))
                        {
                            presenter.ChangeContact(i, person, changeContact.nameTB.Text, changeContact.surnameTB.Text, number);
                        }
                    }
                }
            }

            changeContact.Hide();
            presenter.SavePerson(person);
            person = presenter.ReadPerson();
            UpdateContactListBox();
        }

        public void ChangeContact()
        {
            for (int i = 0; i < person.Contacts.Count; i++)
            {
                if (contactLB.SelectedItem.ToString().Contains(person.Contacts[i].ToString()))
                {
                    changeContact.nameTB.Text = person.Contacts[i].Name;
                    changeContact.surnameTB.Text = person.Contacts[i].Surname;
                    changeContact.numberTB.Text = person.Contacts[i].Phone.ToString();
                    changeContact.ShowDialog();
                }
            }
        }

        public void AddNote()
        {
            bool isAdd = true;
            if (nameNoteTB.Text == string.Empty)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Name of note can't be empty";
                window.ShowDialog();
            }

            else if (noteTB.Text == string.Empty)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Note can't be empty";
                window.ShowDialog();
            }
            else
            {
                foreach (var note in person.Notes)
                {
                    if (note.Title == nameNoteTB.Text)
                    {
                        isAdd = false;
                        dialogWindow window = new dialogWindow();
                        window.dialogMessage.Text = "You can't add notes with the same name";
                        window.ShowDialog();
                        break;
                    }
                }
                if (isAdd)
                {
                    presenter.AddNote(nameNoteTB.Text, noteTB.Text, person);
                    presenter.SavePerson(person);
                    person = presenter.ReadPerson();
                    UpdateNotesList();
                }
            }
        }

        public void RemoveNote()
        {
            if (notesLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose any note";
                window.ShowDialog();
            }

            else
            {
                for (int i = 0; i < person.Notes.Count; i++)
                {
                    if (notesLB.SelectedItem.ToString().Contains(person.Notes[i].Title))
                    {
                        presenter.RemoveNote(i, person);
                        break;
                    }
                }
                presenter.SavePerson(person);
                person = presenter.ReadPerson();
                UpdateNotesList();
            }
        }

        public void ChangeNote()
        {
            if (notesLB.SelectedItem == null)
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "You don't choose any note";
                window.ShowDialog();
            }

            else
            {
                for (int i = 0; i < person.Notes.Count; i++)
                {
                    if (notesLB.SelectedItem.ToString().Contains(person.Notes[i].Title))
                    {
                        presenter.ChangeNote(i, person, nameNoteTB.Text, noteTB.Text);
                        presenter.SavePerson(person);
                        person = presenter.ReadPerson();
                        UpdateNotesList();
                        break;
                    }
                }
            }
        }

        public void SaveNote()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Txt files(*.txt)|*.txt";
            if (fileDialog.ShowDialog() == true)
            {
                presenter.SaveNote(fileDialog.FileName, noteTB.Text);

            }
        }

        public void LoadNote()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Txt files(*.txt)|*.txt|All files(*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                noteTB.Text = presenter.LoadNote(fileDialog.FileName);
            };
        }

        public void HamburgerMenu()
        {
            DoubleAnimation da = new DoubleAnimation();
            if (!isMenuOpen)
            {
                da.From = 32;
                da.To = 250;
                da.Duration = TimeSpan.FromMilliseconds(500);
                isMenuOpen = true;
            }
            else
            {
                da.From = 220;
                da.To = 32;
                da.Duration = TimeSpan.FromMilliseconds(500);
                isMenuOpen = false;
            }

            menuColumn.BeginAnimation(StackPanel.WidthProperty, da);
        }

        public void ShowPerson()
        {
            HideAll();
            personSP_1.Visibility = Visibility.Visible;
            personSP_2.Visibility = Visibility.Visible;
            personAvatarSP.Visibility = Visibility.Visible;
        }

        public void ShowTasks()
        {
            HideAll();
            addTaskBtn.Visibility = Visibility.Visible;
            removeTaskBtn.Visibility = Visibility.Visible;
            changeTaskBtn.Visibility = Visibility.Visible;
            completedTaskBtn.Visibility = Visibility.Visible;
            tasksLB.Visibility = Visibility.Visible;
        }

        public void ShowAchievments()
        {
            HideAll();
            leveLabel_1.Visibility = Visibility.Visible;
            levelLabel_2.Visibility = Visibility.Visible;
            levelPB.Visibility = Visibility.Visible;
            countOfComplitedTasksLabel_1.Visibility = Visibility.Visible;
            countOfComplitedTasksLabel_2.Visibility = Visibility.Visible;
            xpCount.Visibility = Visibility.Visible;
            Xplabel.Visibility = Visibility.Visible;

            levelLabel_2.Content = person.Level;
            countOfComplitedTasksLabel_2.Content = person.CountOfComplitedTasks;
            xpCount.Content = person.XP;
        }

        public void ShowMotivation()
        {
            HideAll();
            motivationGrid.Visibility = Visibility.Visible;
        }

        public void ShowSettings()
        {
            HideAll();
            settingsGrid.Visibility = Visibility.Visible;
            UpdateUsers();
        }

        public void PrintPersonInfo()
        {
            pName.Content = person.Name;
            pSurname.Content = person.Surname;
            pAge.Content = person.Age.ToString();
            pLevel.Content = person.Level;
            levelPB.Value = person.XP;
            levelLabel_2.Content = person.Level;
        }

        public void AddTask()
        {
            addTaskWindow.levelCB.Items.Clear();

            addTaskWindow.levelCB.Items.Add("Low");
            addTaskWindow.levelCB.Items.Add("Normal");
            addTaskWindow.levelCB.Items.Add("High");
            addTaskWindow.levelCB.Items.Add("Important");

            addTaskWindow.ShowDialog();
        }

        public void AddUser()
        {
            addUserWindow.ShowDialog();
            UpdateUsers();
        }

        public void CheckPass()
        {
            if (passwordWindow.passBox.Password == person.Password)
            {
                passwordWindow.Close();
            }

            else
            {
                MessageBox.Show("Incorrect password", "Incorrect password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void PassWindowClosing()
        {
            if (passwordWindow.passBox.Password != person.Password)
            {
                Application.Current.Shutdown();
            }
        }

        public void XPValueChanging()
        {
            if (person.XP <= 20)
            {
                levelPB.Foreground = new SolidColorBrush(Colors.Red);
            }

            else if (person.XP > 20 && person.XP <= 40)
            {
                levelPB.Foreground = new SolidColorBrush(Colors.Orange);
            }

            else if (person.XP > 40 && person.XP <= 60)
            {
                levelPB.Foreground = new SolidColorBrush(Colors.Green);
            }

            else if (person.XP > 60 && person.XP <= 80)
            {
                levelPB.Foreground = new SolidColorBrush(Colors.LightBlue);
            }

            else
            {
                levelPB.Foreground = new SolidColorBrush(Colors.Violet);
            }
        }

        public void ChangeTheme()
        {
            RadialGradientBrush gradientBrush = new RadialGradientBrush();
            if (changeTheme.SelectedItem.ToString() == "Light")
            {
                ResourceDictionary newRes = new ResourceDictionary();
                newRes.Source = new Uri("../Views/Light.xaml", UriKind.RelativeOrAbsolute);

                ResourceDictionary cbRes = new ResourceDictionary();
                cbRes.Source = new Uri("../Views/RBTemplate.xaml", UriKind.RelativeOrAbsolute);

                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(newRes);

                addTaskWindow.Resources.MergedDictionaries.Clear();
                addTaskWindow.Resources.MergedDictionaries.Add(newRes);
                addTaskWindow.Resources.MergedDictionaries.Add(cbRes);

                addUserWindow.Resources.MergedDictionaries.Clear();
                addUserWindow.Resources.MergedDictionaries.Add(newRes);

                changeTaskWindow.Resources.MergedDictionaries.Clear();
                changeTaskWindow.Resources.MergedDictionaries.Add(newRes);

                phraseWindow.Resources.MergedDictionaries.Clear();
                phraseWindow.Resources.MergedDictionaries.Add(newRes);


                changeTheme.Background = new SolidColorBrush(Colors.LightBlue);

                gradientBrush.GradientStops.Add(new GradientStop(Colors.White, 0));
                gradientBrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 1));
                this.Background = gradientBrush;
                addTaskWindow.Background = gradientBrush;
                addUserWindow.Background = gradientBrush;
                changeTaskWindow.Background = gradientBrush;
                phraseWindow.Background = gradientBrush;
                passwordWindow.Background = gradientBrush;
                menuColumn.Background = gradientBrush;
                contactWindow.Background = gradientBrush;

                mcChart.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            else if (changeTheme.SelectedItem.ToString() == "Dark")
            {
                ResourceDictionary newRes = new ResourceDictionary();
                newRes.Source = new Uri("../Views/Dark.xaml", UriKind.RelativeOrAbsolute);

                ResourceDictionary cbRes = new ResourceDictionary();
                cbRes.Source = new Uri("../Views/RBTemplate.xaml", UriKind.RelativeOrAbsolute);

                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(newRes);

                addTaskWindow.Resources.MergedDictionaries.Clear();
                addTaskWindow.Resources.MergedDictionaries.Add(newRes);
                addTaskWindow.Resources.MergedDictionaries.Add(cbRes);

                addUserWindow.Resources.MergedDictionaries.Clear();
                addUserWindow.Resources.MergedDictionaries.Add(newRes);

                changeTaskWindow.Resources.MergedDictionaries.Clear();
                changeTaskWindow.Resources.MergedDictionaries.Add(newRes);

                changeTheme.Background = new SolidColorBrush(Colors.DarkGray);
                changeTheme.Foreground = new SolidColorBrush(Colors.LightBlue);

                gradientBrush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(69, 69, 69), 0));
                gradientBrush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(45, 45, 45), 1));
                this.Background = gradientBrush;
                addTaskWindow.Background = gradientBrush;
                addUserWindow.Background = gradientBrush;
                changeTaskWindow.Background = gradientBrush;
                phraseWindow.Background = gradientBrush;
                passwordWindow.Background = gradientBrush;
                menuColumn.Background = gradientBrush;
                mcChart.Background = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Please choose a theme";
                window.ShowDialog();
            }
        }

        public void AddData()
        {
            phraseWindow.ShowDialog();
        }

        public void ShowDiagram()
        {
            HideAll();

            salaryLabel.Visibility = Visibility.Visible;
            foodCostLabel.Visibility = Visibility.Visible;
            utilitiesCostLabel.Visibility = Visibility.Visible;
            personalCostLabel.Visibility = Visibility.Visible;
            salaryTB.Visibility = Visibility.Visible;
            foodCostTB.Visibility = Visibility.Visible;
            utilitiesCostTB.Visibility = Visibility.Visible;
            personalCostTB.Visibility = Visibility.Visible;
            foodPercentCostLabel.Visibility = Visibility.Visible;
            foodPercentCostLabel_2.Visibility = Visibility.Visible;
            utilitiesPercentCostLabel.Visibility = Visibility.Visible;
            utilitiesPercentCostLabel_2.Visibility = Visibility.Visible;
            personalPercentCostLabel.Visibility = Visibility.Visible;
            personalPercentCostLabel_2.Visibility = Visibility.Visible;
            calculateCostBtn.Visibility = Visibility.Visible;
            leftMoneyPercentLabel.Visibility = Visibility.Visible;
            leftMoneyPercentLabel_2.Visibility = Visibility.Visible;
            chartGrid.Visibility = Visibility.Visible;
        }

        public void Exit()
        {
            passwordWindow.Close();
            addTaskWindow.Close();
            addUserWindow.Close();
            changeTaskWindow.Close();
            phraseWindow.Close();
            Application.Current.Shutdown();
        }

        public void AddContact()
        {
            contactWindow.ShowDialog();
        }

        public void ShowContacts()
        {
            HideAll();
            contactGrid.Visibility = Visibility.Visible;
            UpdateContactListBox();
        }

        public void ShowNotes()
        {
            HideAll();
            notesGrid.Visibility = Visibility.Visible;
            UpdateNotesList();
        }

        public void ShowMoneyExchange()
        {
            HideAll();
            moneyExchangeGrid.Visibility = Visibility.Visible;
            presenter.ShowCurrencies();
        }

        public void ShowCurrencies()
        {
            try
            {
                WebClient webClient = new WebClient();
                string url = "http://api.exchangeratesapi.io/v1/latest?access_key=e24c1bfa1ed12df197d85e097991732f";
                var json = webClient.DownloadString(url);
                currencies = JsonConvert.DeserializeObject<Currency>(json);
                foreach (var currency in currencies.Rates)
                {
                    valueOneCB.Items.Add(currency.Key);
                    valueTwoCB.Items.Add(currency.Key);
                }
                valueOneCB.SelectedItem = "USD";
                valueTwoCB.SelectedItem = "USD";
            }
            catch (Exception ex)
            {
                dialogWindow dialogWindow = new dialogWindow();
                dialogWindow.dialogMessage.Text = "Please connect to the network for using currencies feature";
                dialogWindow.ShowDialog();
            }

        }

        public void CalculateCurrency()
        {
            try
            {
                if (valueOneTB.Text == string.Empty)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Money value can't be empty";
                    window.ShowDialog();
                }
                else
                {
                    double value;
                    if (double.TryParse(valueOneTB.Text, out value))
                    {
                        double valueCur = 1;
                        double changeCur = 1;
                        foreach (var cur in currencies.Rates)
                        {
                            if (valueOneCB.SelectedItem.ToString() == cur.Key)
                            {
                                valueCur = cur.Value;
                            }
                            if (valueTwoCB.SelectedItem.ToString() == cur.Key)
                            {
                                changeCur = cur.Value;
                            }
                        }
                        resultExchangeLabel.Content = presenter.CalculateCurrency(value, valueCur, changeCur);

                    }
                    else
                    {
                        dialogWindow window = new dialogWindow();
                        window.dialogMessage.Text = "Money value is not a number";
                        window.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                dialogWindow dialogWindow = new dialogWindow();
                dialogWindow.dialogMessage.Text = "Please connect to the network for using currencies feature";
                dialogWindow.ShowDialog();
            }

        }

        private void calculateMoneyBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.CalculateCurrency();
        }

        public void ShowWeather()
        {
            try
            {
                Weather weather = presenter.GetWeatherData();
                regionLabel.Content = weather.Location.Region;
                temperatureLabel.Content = weather.Current.Temperature;
                temperatureLabel.Content += " \u00B0C";
                windSpeedLabel.Content = weather.Current.WindSpeed;
                windSpeedLabel.Content += " m/s";
                visibilityLabel.Content = weather.Current.Visibility;
                visibilityLabel.Content += " km";
                cloudcoverLabel.Content = weather.Current.CloudCover;
                humidityLabel.Content = weather.Current.Humidity;

                WeatherStates weatherStates = CalculateWeatherState(weather);

                if (weatherStates == WeatherStates.clear)
                {
                    weatherImg.Source = new BitmapImage(new Uri(@"../Images/sunnyWeatherImg.png", UriKind.RelativeOrAbsolute));
                    weatherLabel.Content = "Clear";
                }

                else if (weatherStates == WeatherStates.partlyCloudy)
                {
                    weatherImg.Source = new BitmapImage(new Uri(@"../Images/CloudySunnyWeatherImg.png", UriKind.RelativeOrAbsolute));
                    weatherLabel.Content = "Partly Cloudy";
                }

                else if (weatherStates == WeatherStates.cloudy)
                {
                    weatherImg.Source = new BitmapImage(new Uri(@"../Images/CloudyWeatherImg.png", UriKind.RelativeOrAbsolute));
                    weatherLabel.Content = "Cloudy";
                }
                else if (weatherStates == WeatherStates.rainy)
                {
                    weatherImg.Source = new BitmapImage(new Uri(@"../Images/RainyWeatherImg.png", UriKind.RelativeOrAbsolute));
                    weatherLabel.Content = "Rainy";
                }
                else
                {
                    weatherImg.Source = new BitmapImage(new Uri(@"../Images/SnowyWeatherImg.png", UriKind.RelativeOrAbsolute));
                    weatherLabel.Content = "Snowy";
                }
            }
            catch (Exception)
            {
                dialogWindow dialogWindow = new dialogWindow();
                dialogWindow.dialogMessage.Text = "Please connect to the network for using weather feature";
                dialogWindow.ShowDialog();
            }

        }

        private void weatherBtn_Click(object sender, RoutedEventArgs e)
        {
            HideAll();
            weatherGrid.Visibility = Visibility.Visible;
        }

        public WeatherStates CalculateWeatherState(Weather weather)
        {
            WeatherStates weatherStates = WeatherStates.clear;
            if (weather.Current.CloudCover <= 15)
            {
                weatherStates = WeatherStates.clear;
            }
            else if (weather.Current.CloudCover > 15 && weather.Current.CloudCover <= 30)
            {
                weatherStates = WeatherStates.partlyCloudy;
            }
            else if (weather.Current.CloudCover > 30 && weather.Current.Humidity <= 50)
            {
                weatherStates = WeatherStates.cloudy;
            }
            else if (weather.Current.CloudCover > 30 && weather.Current.Humidity > 50 && weather.Current.Temperature >= 0)
            {
                weatherStates = WeatherStates.rainy;
            }
            else if (weather.Current.CloudCover > 30 && weather.Current.Humidity > 50 && weather.Current.Temperature < 0)
            {
                weatherStates = WeatherStates.snowy;
            }

            return weatherStates;
        }

        public void ChangeAvatar()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Png files(*.png)|*.png|Jpg files(*.jpg)|*.jpg|Ico files(*.ico)|*.ico|All files(*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                person.Avatar = fileDialog.FileName;
                presenter.SavePerson(person);
            }
        }

        public void RemoveAvatar()
        {
            person.Avatar = @"..\..\..\Images\personImg.png";
            presenter.SavePerson(person);
        }

        private void changeAvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ChangeAvatar();
            presenter.RestartApp();
        }

        private void removeAvatarBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.RemoveAvatar();
            presenter.RestartApp();
        }

        public void RestartApp()
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void addUserInPassWindow(object sender, RoutedEventArgs e)
        {
            presenter.AddUserInPassWindow();
        }

        public void AddUserInPassWindow()
        {
            addUserWindow.addBtn.Click += AddUserInPassWindow_Click;
            addUserWindow.ShowDialog();
        }

        public void AddUserInPassWindow_Click()
        {
            People people = new People();

            bool isAdd = true;
            foreach (var p in people.people)
            {
                if (addUserWindow.nameTB.Text == p.Name)
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "You can't add users with the same name";
                    window.ShowDialog();
                    isAdd = false;
                    break;
                }
            }

            if (String.IsNullOrWhiteSpace(addUserWindow.nameTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Name field is empty";
                window.ShowDialog();
            }

            else if (String.IsNullOrWhiteSpace(addUserWindow.surnameTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Surname field is empty";
                window.ShowDialog();
            }

            else if (String.IsNullOrWhiteSpace(addUserWindow.ageTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Age field is empty";
                window.ShowDialog();
            }
            else if (String.IsNullOrWhiteSpace(addUserWindow.passTB.Text))
            {
                isAdd = false;
                dialogWindow window = new dialogWindow();
                window.dialogMessage.Text = "Password field is empty";
                window.ShowDialog();
            }


            if (isAdd)
            {
                int age;
                if (!int.TryParse(addUserWindow.ageTB.Text, out age))
                {
                    dialogWindow window = new dialogWindow();
                    window.dialogMessage.Text = "Age is not a number";
                    window.ShowDialog();
                    age = 0;
                }
                else
                {

                }
                presenter.AddUser(addUserWindow.nameTB.Text, addUserWindow.surnameTB.Text, age, addUserWindow.passTB.Text);

                People updatedPeople = new People();
                foreach (var p in updatedPeople.people)
                {
                    if (addUserWindow.nameTB.Text == p.Name)
                    {
                        this.person = p;
                    }
                }


                presenter.SavePerson(person);

                addUserWindow.Close();
                RestartApp();
            };
        }

        public void AddUserInPassWindow_Click(object sender, RoutedEventArgs e)
        {
            presenter.AddUserInPassWindow_Click();
        }

        private void helpBtn_Click(object sender, RoutedEventArgs e)
        {
            presenter.ShowManual();
        }

        public void ShowManual()
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void salaryTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.CheckCalcCurrenciesBtnEnabled();
        }

        public void CheckCalcCurrenciesBtnEnabled()
        {
            if (String.IsNullOrWhiteSpace(salaryTB.Text) || String.IsNullOrWhiteSpace(foodCostTB.Text) ||
                String.IsNullOrWhiteSpace(utilitiesCostTB.Text) || String.IsNullOrWhiteSpace(personalCostTB.Text))
            {
                calculateCostBtn.IsEnabled = false;
            }
            else
            {
                calculateCostBtn.IsEnabled = true;
            }
        }

        private void foodCostTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.CheckCalcCurrenciesBtnEnabled();

        }

        private void utilitiesCostTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.CheckCalcCurrenciesBtnEnabled();

        }

        private void personalCostTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            presenter.CheckCalcCurrenciesBtnEnabled();

        }
    }
}
