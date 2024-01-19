using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Entities
{
    public class Candidate
    {
        public int Id { get; set; }
        public Guid Number { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
