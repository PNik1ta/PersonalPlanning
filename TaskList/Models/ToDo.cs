using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    [Serializable]
    class ToDo
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
        public Level Level { get; set; }
        public string Comment { get; set; }
        public bool? isNotify { get; set; }
        public override string ToString()
        {
            return $"{Name} ({Date}) Priority: {Level} ({Comment})";
        }
    }
}
