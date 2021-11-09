using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class AddSlotsRequestDTO
    {
        public DateTime BookingFrom { get; set; }
        public DateTime BookingTo { get; set; }

    }
}
