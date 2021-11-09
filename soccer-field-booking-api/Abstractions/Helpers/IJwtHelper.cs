using Microsoft.AspNetCore.Http;
using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Abstractions.Helpers
{
    public interface IJwtHelper
    {
        string CreateToken(User user);
        int GetUserIdFromBearerToken(string bearerToken);
        string GetBearerToken(HttpRequest request);
        
    }
}
