using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    [Serializable]
    class Note
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
