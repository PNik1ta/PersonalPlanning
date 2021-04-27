using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    public class Phrase
    {
        public int Id { get; set; }
        public string Author { get; set; }
        [Column("Phrase")]
        public string AuthorPhrase { get; set; }

        public override string ToString()
        {
            return $"{Author}: {AuthorPhrase}";
        }
    }
}
