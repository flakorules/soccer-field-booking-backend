using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class AuthenticateUserResponseDTO
    {
        public int UserId { get; set; }        
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
