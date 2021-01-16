﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    interface IView
    {
        void UpdateProgress();
        void UpdateContactListBox();
        void UpdateDB();
        void UpdateUsers();
        void HideAll();
        void UpdateTasks();
        void UpdateNotesList();
        void AddTask();
        void AddTaskEvent();
        void ChangeUser();
        void RemoveUser();
        void RemoveTask();
        void AddUser();
        void AddUserEvent();
        void ChangeTask();
        void ChangeTaskEvent();
        void CompleteTask();
        void RemoveData();
        void AddData();
        void AddDataEvent();
        void CalculateCost();
        void Notify();
        void AddContact();
        void AddContactEvent();
        void RemoveContact();
        void ChangeContactEvent();
        void ChangeContact();
        void AddNote();
        void RemoveNote();
        void ChangeNote();
        void SaveNote();
        void LoadNote();
        void HamburgerMenu();
        void ShowPerson();
        void ShowTasks();
        void ShowAchievments();
        void ShowMotivation();
        void ShowSettings();
        void PrintPersonInfo();
        void CheckPass();
        void PassWindowClosing();
        void XPValueChanging();
        void ChangeTheme();
        void ShowDiagram();
        void Exit();
        void ShowContacts();
        void ShowNotes();
        void ShowMoneyExchange();
        void ShowCurrencies();
        void CalculateCurrency();
        void ShowWeather();
        WeatherStates CalculateWeatherState(Weather weather);
    }
}
