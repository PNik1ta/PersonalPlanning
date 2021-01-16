using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    [Serializable]
    class People : IReadWriteFile
    {
        public People()
        {
            Read();
        }

        public List<Person> people = new List<Person>();

        public void AddPerson(Person p)
        {
            people.Add(p);
        }

        public void RemovePerson(Person p)
        {
            bool res = people.Remove(p);
        }

        public void Read()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("people.bin", FileMode.Open))
            {
                this.people = formatter.Deserialize(fs) as List<Person>;
            }
        }

        public void Write()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using(FileStream fs=new FileStream("people.bin",FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, people);
            }
        }
    }
}
