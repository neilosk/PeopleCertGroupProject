using FinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Dtos
{
    public class CandidateDto
    {
        public Guid Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime BirthDate { get; set;}
        public string Password { get; set; }

        public static CandidateDto FromEntity(Candidate candidate)
        {
            return new CandidateDto
            {
                Number = candidate.Number,
                FirstName = candidate.User.FirstName,
                LastName = candidate.User.LastName,
                Phone = candidate.User.Phone,
                Email = candidate.User.Email,
                Address = candidate.User.Address,
                BirthDate = candidate.BirthDate
            };
        }
    }
}
