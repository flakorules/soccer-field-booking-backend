using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerFieldBooking.API.Abstractions.Repository;
using SoccerFieldBooking.API.DTO;
using SoccerFieldBooking.API.Exceptions.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequestDTO request)
        {
            try
            {
                var response = await _userRepository.CreateUser(request);
                return CreatedAtAction(nameof(GetUser), new { userId = response.Data.UserId }, response);
                          
            }
            catch (GenericRepositoryException exception)
            {
                return BadRequest(new GenericResponseDTO<CreateUserRequestDTO>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                });
                
            }            

        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int userId)
        {
            try
            {
                var response = _userRepository.GetUser(userId);
                return Ok(response);

            }
            catch (GenericRepositoryException exception)
            {

                var response = new GenericResponseDTO<GetUserResponseDTO>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };

                return NotFound(response);
            }

        }

        [HttpPost("authenticate")]
        public IActionResult AuthenticateUser(AuthenticateUserRequestDTO request)
        {
            try
            {
                var response = _userRepository.AuthenticateUser(request);
                return Ok(response);
            }
            catch (GenericRepositoryException exception)
            {
                var response = new GenericResponseDTO<AuthenticateUserResponseDTO>()
                {
                    ErrorCode = exception.ErrorCode,
                    Message = exception.Message,
                    Data = null
                };
                return NotFound(response);
            }
        }

        
    }
}
