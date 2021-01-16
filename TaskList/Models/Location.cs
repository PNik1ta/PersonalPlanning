using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Location
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public double Lattitude { get; set; }
        public double Longittude { get; set; }
        public string Timezone_Id { get; set; }
        public DateTime LocalTime { get; set; }
        public double Utc_Offset { get; set; }
    }
}
