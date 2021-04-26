using Microsoft.Xaml.Behaviors.Media;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web.WebSockets;
using System.Windows;
using TaskList.Models;
using TaskList.Views;

namespace TaskList.Presenters
{
    class AppViewPresenter
    {
        public IView view;

        public void AddTask(string name, string comment, DateTime? date, Level level, bool? isNotify, Person person)
        {
            ToDo toDo = new ToDo();
            toDo.Name = name;
            toDo.Level = level;
            toDo.Comment = comment;
            toDo.Date = date;
            toDo.isNotify = isNotify;
            person.ToDos.Add(toDo);
        }

        public void Save(string name, string surname, int age, string password)
        {
            People people = new People();
            Person person = new Person();

            person.Name = name;
            person.Surname = surname;
            person.Age = age;
            person.Password = password;
            people.AddPerson(person);
            people.Write();
        }

        public void RemoveTask(int index, Person person)
        {
            person.ToDos.RemoveAt(index);
        }

        public void SavePerson(Person person)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("user.bin", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, person);
            }
        }

        public Person ReadPerson()
        {
            Person person = new Person();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("user.bin", FileMode.Open))
            {
                person = formatter.Deserialize(fs) as Person;
            }
            return person;
        }
        public void AddUser(string pName, string pSurname, int pAge, string pass)
        {
            People people = new People();
            Person person = new Person();
            person.Name = pName;
            person.Surname = pSurname;
            person.Age = pAge;
            person.Password = pass;
            people.AddPerson(person);
            people.Write();
        }
        public void ChangeTask(Person person, int index, string name, string comment, DateTime? date, Level level)
        {
            person.ToDos[index].Name = name;
            person.ToDos[index].Comment = comment;
            person.ToDos[index].Date = date;
            person.ToDos[index].Level = level;
        }
        public void CompleteTask(Person person)
        {
            person.LevelUp();
            person.XPUp();
        }
        public void RemoveData(SqlConnection connection, string connectionString, string content)
        {
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@id", int.Parse(content));
            command.CommandText = "delete from Motivation where Id=@id";
            connection.Open();
            command.ExecuteNonQuery();

        }

        public void AddData(SqlConnection connection, string connectionString, string authorText, string phraseText)
        {
            ;
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@author", authorText);
            command.Parameters.AddWithValue("@phrase", phraseText);
            command.CommandText = "insert into Motivation (Author,Phrase) values (@author,@phrase)";
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void AddContact(string name, string surname, int phone, Person person)
        {
            Contact contact = new Contact();
            contact.Name = name;
            contact.Surname = surname;
            contact.Phone = phone;
            person.Contacts.Add(contact);
        }
        public void RemoveContact(int index, Person person)
        {
            person.Contacts.RemoveAt(index);
        }
        public void ChangeContact(int index, Person person, string name, string surname, int phone)
        {
            person.Contacts[index].Name = name;
            person.Contacts[index].Surname = surname;
            person.Contacts[index].Phone = phone;
        }
        public void AddNote(string title, string text, Person person)
        {
            Note note = new Note();
            note.Text = text;
            note.Title = title;
            person.Notes.Add(note);
        }

        internal void HamburgerMenu()
        {
            view.HamburgerMenu();
        }

        internal void ShowPerson()
        {
            view.ShowPerson();
        }

        public void RemoveNote(int index, Person person)
        {
            person.Notes.RemoveAt(index);
        }

        internal void ShowTasks()
        {
            view.ShowTasks();
        }

        internal void ShowAchievments()
        {
            view.ShowAchievments();
        }

        internal void ShowMotivation()
        {
            view.ShowMotivation();
        }

        public void ChangeNote(int index, Person person, string text, string title)
        {
            person.Notes[index].Text = text;
            person.Notes[index].Title = title;
        }

        internal void ShowSettings()
        {
            view.ShowSettings();
        }

        internal void PrintPersonInfo()
        {
            view.PrintPersonInfo();
        }

        public void SaveNote(string path, string text)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(text);
            }
        }

        internal void AddTask()
        {
            view.AddTask();
        }

        public string LoadNote(string path)
        {
            string text = string.Empty;
            using (StreamReader reader = new StreamReader(path))
            {
                text = reader.ReadToEnd();
            }
            return text;
        }

        internal void AddUser()
        {
            view.AddUser();
        }

        public void RemoveUser(int index)
        {
            People people = new People();
            people.people.RemoveAt(index);
            people.Write();
        }

        internal void CheckPass()
        {
            view.CheckPass();
        }

        internal void AddTaskEvent()
        {
            view.AddTaskEvent();
        }

        internal void PassWindowClosing()
        {
            view.PassWindowClosing();
        }

        internal void ChangeUser()
        {
            view.ChangeUser();
        }

        internal void RemoveUser()
        {
            view.RemoveUser();
        }

        internal void RemoveTask()
        {
            view.RemoveTask();
        }

        internal void AddUserEvent()
        {
            view.AddUserEvent();
        }

        internal void ChangeTask()
        {
            view.ChangeTask();
        }

        internal void XPValueChanging()
        {
            view.XPValueChanging();
        }

        internal void ChangeTaskEvent()
        {
            view.ChangeTaskEvent();
        }

        internal void ChangeTheme()
        {
            view.ChangeTheme();
        }

        internal void UpdateDB()
        {
            view.UpdateDB();
        }

        internal void CompleteTask()
        {
            view.CompleteTask();
        }

        internal void RemoveData()
        {
            view.RemoveData();
        }

        internal void ShowMoneyExchange()
        {
            view.ShowMoneyExchange();
        }

        internal void AddDataEvent()
        {
            view.AddDataEvent();
        }

        internal void AddData()
        {
            view.AddData();
        }

        internal void CalculateCost()
        {
            view.CalculateCost();
        }

        internal void Notify()
        {
            view.Notify();
        }

        internal void ShowDiagram()
        {
            view.ShowDiagram();
        }

        internal void AddContactEvent()
        {
            view.AddContactEvent();
        }

        internal void RemoveContact()
        {
            view.RemoveContact();
        }

        internal void ChangeContactEvent()
        {
            view.ChangeContactEvent();
        }

        internal void Exit()
        {
            view.Exit();
        }

        internal void ChangeContact()
        {
            view.ChangeContact();
        }

        internal void AddContact()
        {
            view.AddContact();
        }

        internal void AddNote()
        {
            view.AddNote();
        }

        internal void RemoveNote()
        {
            view.RemoveNote();
        }

        internal void ChangeNote()
        {
            view.ChangeNote();
        }

        internal void SaveNote()
        {
            view.SaveNote();
        }

        internal void LoadNote()
        {
            view.LoadNote();
        }

        internal void ShowContacts()
        {
            view.ShowContacts();
        }

        internal void ShowNotes()
        {
            view.ShowNotes();
        }

        internal void ShowCurrencies()
        {
            view.ShowCurrencies();
        }

        internal double CalculateCurrency(double value, double valueCurrency, double changeCurrency)
        {
            return value / valueCurrency * changeCurrency;
        }

        internal void CalculateCurrency()
        {
            view.CalculateCurrency();
        }

        internal Weather GetWeatherData()
        {
            string externalIp = new WebClient().DownloadString("https://icanhazip.com");
            Console.WriteLine(externalIp);
            string link = "http://api.ipstack.com/" + externalIp + "?access_key=f95743aa8941816e833b1e89e601b7dd&format=1";
            string info = new WebClient().DownloadString(link);
            IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
            string weatherLink = "http://api.weatherstack.com/current?access_key=d5119705bf9d6509bad3fb30f4d4f9e1&query=" + ipInfo.Location.Capital;
            string weatherData = new WebClient().DownloadString(weatherLink);
            Weather weather = JsonConvert.DeserializeObject<Weather>(weatherData);
            return weather;


        }

        internal void ShowWeather()
        {
            view.ShowWeather();
        }

        internal void ChangeAvatar()
        {
            view.ChangeAvatar();
        }

        internal void RemoveAvatar()
        {
            view.RemoveAvatar();
        }

        internal void RestartApp()
        {
            view.RestartApp();
        }

        internal void AddUserInPassWindow()
        {
            view.AddUserInPassWindow();
        }

        internal void AddUserInPassWindow_Click()
        {
            view.AddUserInPassWindow_Click();
        }

        internal void ShowManual()
        {
            view.ShowManual();
        }

        internal void CheckCalcCurrenciesBtnEnabled()
        {
            view.CheckCalcCurrenciesBtnEnabled();
        }
    }
}
