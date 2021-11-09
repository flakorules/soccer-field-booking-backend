using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class AuthenticateUserRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
