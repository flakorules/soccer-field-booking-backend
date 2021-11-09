using AutoMapper;
using Microsoft.Data.SqlClient;
using SoccerFieldBooking.API.Abstractions.Helpers;
using SoccerFieldBooking.API.Abstractions.Repository;
using SoccerFieldBooking.API.DTO;
using SoccerFieldBooking.API.Entities;
using SoccerFieldBooking.API.Exceptions.Repository;
using SoccerFieldBooking.API.Persistency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Repository
{
    public class BookingRepository : RepositoryBase, IBookingRepository
    {
        private readonly IJwtHelper _jwtHelper;

        public BookingRepository(SoccerFieldBookingDBContext context, IMapper mapper, IJwtHelper jwtHelper) : base(context, mapper)
        {
            _jwtHelper = jwtHelper;
        }

        public GenericResponseDTO<List<Booking>> AddSlots(AddSlotsRequestDTO request)
        {
            try
            {
                List<Booking> foundBookings;

                if (request.BookingFrom.Date < DateTime.Now.Date)
                {
                    throw new GenericRepositoryException("009", "DateFrom should be equals than today or greater");
                }

                if (request.BookingFrom > request.BookingTo)
                {
                    throw new GenericRepositoryException("009", "BookingFrom should be less or equal to BookingTo");
                }

                foundBookings = _context.Bookings.Where(booking =>
                    booking.BookingFrom >= request.BookingFrom &&
                    booking.BookingFrom <= request.BookingTo).ToList();

                if (foundBookings.Count > 0)
                {
                    throw new GenericRepositoryException("008", "Thre are already slots on range");
                }                

                List<DateTime> dates = GetDates(request.BookingFrom, request.BookingTo);
                List<Booking> slots = GetSlots(dates);
                _context.Bookings.AddRangeAsync(slots);
                int rowsAffected = _context.SaveChanges();

                return new GenericResponseDTO<List<Booking>>()
                {
                    ErrorCode = rowsAffected > 0 ? "000" : "007",
                    Message = rowsAffected > 0 ? $"{slots.Count} were added successfully"
                        : "Slots were not added",
                    Data = slots
                };
                                
            }
            catch (SqlException exception)
            {
                return new GenericResponseDTO<List<Booking>>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        private List<GetSlotsResponseDTO> GetSlots(Func<Booking,bool> condition)
        {
            var availableSlots = _context.Bookings.Where(condition).ToList();

            return availableSlots.GroupBy(booking => booking.BookingFrom.Date)
                    .Select(o => new GetSlotsResponseDTO()
                    {
                        Date = o.Key,
                        Slots = availableSlots.Where(p => p.BookingFrom.Date.Equals(o.Key)).ToList()
                    }).ToList();
        }


        public GenericResponseDTO<List<GetSlotsResponseDTO>> GetAvailableSlots()
        {
            try
            {
                static bool condition(Booking booking) => booking.UserId == null;
                var availableSlotsByDate = GetSlots(condition);

                return new GenericResponseDTO<List<GetSlotsResponseDTO>>() {

                    ErrorCode = "000",
                    Message = $"{availableSlotsByDate.Count} available slots",
                    Data = availableSlotsByDate

                };
            }
            catch (SqlException exception)
            {
                return new GenericResponseDTO<List<GetSlotsResponseDTO>>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        public GenericResponseDTO<List<GetSlotsResponseDTO>> GetSlotsByUser(string bearerToken)
        {
            try
            {
                

                bool condition(Booking booking) => booking.UserId == _jwtHelper.GetUserIdFromBearerToken(bearerToken);

                var availableSlotsByDate = GetSlots(condition);               

                return new GenericResponseDTO<List<GetSlotsResponseDTO>>()
                {
                    ErrorCode = "000",
                    Message = $"{availableSlotsByDate.Count} available slots",
                    Data = availableSlotsByDate
                };
            }
            catch (SqlException exception)
            {
                return new GenericResponseDTO<List<GetSlotsResponseDTO>>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        public GenericResponseDTO<Booking> BookSlot(int bookingId, string bearerToken)
        {
            try
            {
                var found = _context.Bookings.FirstOrDefault(booking => booking.BookingId == bookingId);

                if (found == null)
                {
                    throw new GenericRepositoryException("008", $"{bookingId} doesn't exist on Database");
                }

                if (found.UserId != null) 
                {
                    throw new GenericRepositoryException("008", $"{found.BookingId} was already booked by another user");
                }

                found.UserId = _jwtHelper.GetUserIdFromBearerToken(bearerToken);

                _context.SaveChanges();

                return new GenericResponseDTO<Booking>()
                {
                    ErrorCode = "000",
                    Message = $"bookingId {found.BookingId} was succesfully booked by userId {found.UserId}",
                    Data = found
                };
            }
            catch (SqlException exception)
            {

                return new GenericResponseDTO<Booking>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        public GenericResponseDTO<Booking> CancelBooking(int bookingId, string bearerToken)
        {
            try
            {
                var found = _context.Bookings.FirstOrDefault(booking => booking.BookingId == bookingId);

                if (found == null)
                {
                    throw new GenericRepositoryException("008", $"{bookingId} doesn't exist on Database");
                }

                if (found.UserId != _jwtHelper.GetUserIdFromBearerToken(bearerToken))
                {
                    throw new GenericRepositoryException("009", $"{found.BookingId} was booked by another user");
                }

                if (found.UserId ==null)
                {
                    throw new GenericRepositoryException("010", $"{found.BookingId} is not booked");
                }

                found.UserId = null;

                _context.SaveChanges();

                return new GenericResponseDTO<Booking>()
                {
                    ErrorCode = "000",
                    Message = $"bookingId {found.BookingId} was succesfully canceled by userId {found.UserId}",
                    Data = found
                };
            }
            catch (SqlException exception)
            {

                return new GenericResponseDTO<Booking>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        private static List<DateTime> GetDates(DateTime from, DateTime to)
        {

            DateTime currentDate = from;
            List<DateTime> dates = new List<DateTime>();

            while (currentDate <= to)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dates;

        }

        private static List<Booking> GetSlots(List<DateTime> dates) 
        {
            List<Booking> bookings = new List<Booking>();
            DateTime from;
            Booking bookingSlot;

            dates.ForEach(date => {

                from = date.AddHours(10);

                for (int i = 1; i <= 12; i++)
                {
                    bookingSlot = new Booking()
                    {
                        BookingFrom = from,
                        BookingTo = from.AddHours(1)
                    };
                    bookings.Add(bookingSlot);
                    from = from.AddHours(1);
                };

            });

            return bookings;
        }
               
    }
}
