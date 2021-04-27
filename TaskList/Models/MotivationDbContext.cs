using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskList.Models
{
    class MotivationDbContext : DbContext
    {
        public MotivationDbContext():base("DefaultConnection")
        {

        }
        public DbSet<Phrase> Phrases { get; set; }
    }
}
