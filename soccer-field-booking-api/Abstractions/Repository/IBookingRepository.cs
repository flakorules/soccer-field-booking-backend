using SoccerFieldBooking.API.DTO;
using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Abstractions.Repository
{
    public interface IBookingRepository
    {
        GenericResponseDTO<List<Booking>> AddSlots(AddSlotsRequestDTO request);
        GenericResponseDTO<List<GetSlotsResponseDTO>> GetAvailableSlots();
        GenericResponseDTO<List<GetSlotsResponseDTO>> GetSlotsByUser(string bearerToken);
        GenericResponseDTO<Booking> BookSlot(int bookingId, string bearerToken);
        GenericResponseDTO<Booking> CancelBooking(int bookingId, string bearerToken);
    }
}
