using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    [Serializable]
    class Contact
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Phone { get; set; }

        public override string ToString()
        {
            return $"{Name} {Surname}: {Phone}";
        }
    }
}
