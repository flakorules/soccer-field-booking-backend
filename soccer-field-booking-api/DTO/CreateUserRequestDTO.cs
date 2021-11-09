using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class CreateUserRequestDTO
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }
        
        public string Name { get; set; }
    }
}
