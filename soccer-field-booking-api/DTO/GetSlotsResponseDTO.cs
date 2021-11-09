using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class GetSlotsResponseDTO
    {
        public DateTime Date { get; set; }
        public List<Booking> Slots { get; set; }
    }
}
