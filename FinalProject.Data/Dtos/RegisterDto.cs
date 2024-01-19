using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Data.Dtos
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }




    }

    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
