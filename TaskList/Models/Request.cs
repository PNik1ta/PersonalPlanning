using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Request
    {
        public string Type { get; set; }
        public string Query { get; set; }
        public string Language { get; set; }
        public string Unit { get; set; }
    }
}
