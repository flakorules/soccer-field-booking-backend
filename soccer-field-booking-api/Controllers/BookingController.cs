using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerFieldBooking.API.Abstractions.Helpers;
using SoccerFieldBooking.API.Abstractions.Repository;
using SoccerFieldBooking.API.DTO;
using SoccerFieldBooking.API.Entities;
using SoccerFieldBooking.API.Exceptions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IJwtHelper _jwtHelper;

        public BookingController(IBookingRepository bookingRepository, IJwtHelper jwtHelper)
        {
            _bookingRepository = bookingRepository;
            _jwtHelper = jwtHelper;
        }

        [HttpPost]
        public IActionResult AddSlots(AddSlotsRequestDTO request)
        {
            try
            {
                return Ok(_bookingRepository.AddSlots(request));
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<List<Booking>>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }

        [HttpGet("available")]
        public IActionResult GetAvailableSlots()
        {
            try
            {
                var response = _bookingRepository.GetAvailableSlots();
                return Ok(response);
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<List<GetSlotsResponseDTO>>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }

        [HttpGet("user")]
        public IActionResult GetSlotsByuser()
        {
            try
            {
                var bearerToken = _jwtHelper.GetBearerToken(Request);
                var response = _bookingRepository.GetSlotsByUser(bearerToken);
                return Ok(response);
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<List<GetSlotsResponseDTO>>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }

        [HttpPut("book/{bookingId}")]
        public IActionResult BookSlot(int bookingId)
        {
            try
            {
                var bearerToken = _jwtHelper.GetBearerToken(Request);
                var response = _bookingRepository.BookSlot(bookingId, bearerToken);
                return Ok(response);
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<Booking>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }

        [HttpPut("cancel/{bookingId}")]
        public IActionResult CancelBooking(int bookingId)
        {
            try
            {
                var bearerToken = _jwtHelper.GetBearerToken(Request);
                var response = _bookingRepository.CancelBooking(bookingId, bearerToken);
                return Ok(response);
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<Booking>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return BadRequest(response);
            }
        }
    }
}
