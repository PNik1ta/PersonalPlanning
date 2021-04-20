using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskList.Models
{
    [Serializable]
    class Person
    {
        public Person()
        {
            this.Password = "admin";
            this.Name = "Unknown";
            this.Surname = "Unknown";
            this.Age = 18;
            this.Notes = new List<Note>();
            this.ToDos = new List<ToDo>();
            this.Contacts = new List<Contact>();
            this.Password = "admin";
            this.Level = 0;
            this.XP = 0;
            this.CountOfComplitedTasks = 0;
            this.Avatar = @"..\..\..\Images\personImg.png";
        }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int CountOfComplitedTasks { get; set; }
        public List<ToDo> ToDos { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Note> Notes { get; set; }
        public int Level { get; set; }
        public int XP { get; set; }
        public string Avatar { get; set; }

        public void LevelUp()
        {
            if (XP >= 100)
            {
                Level++;
                XP -= 100;
            }
        }

        public void XPUp()
        {
            XP += 10;
            CountOfComplitedTasks++;
        }
    }
}
