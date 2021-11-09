using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Abstractions.Helpers
{
    public interface IEncryptionHelper
    {
        string EncryptString(string plainText);
    }
}
