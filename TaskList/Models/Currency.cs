using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Currency
    {
        public Dictionary<string,double> Rates { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
    }
}
