using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Weather
    {
        public Request Request { get; set; }
        public Location Location { get; set; }
        public Current Current { get; set; }
    }
}
