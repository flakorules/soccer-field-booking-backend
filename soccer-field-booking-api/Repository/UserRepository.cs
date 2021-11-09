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
    public class UserRepository : RepositoryBase, IUserRepository
    {

        private readonly IEncryptionHelper _encriptionHelper;
        private readonly IJwtHelper _jwtHelper;
        public UserRepository(SoccerFieldBookingDBContext context, IMapper mapper, IEncryptionHelper encriptionHelper, IJwtHelper jwtHelper) :base(context, mapper)
        {
            _encriptionHelper = encriptionHelper;
            _jwtHelper = jwtHelper;
        }

        public async Task<GenericResponseDTO<CreateUserResponseDTO>> CreateUser(CreateUserRequestDTO request)
        {
            try
            {
                var foundUser = _context.Users.FirstOrDefault(user => user.UserName == request.UserName);

                if(foundUser != null)
                {
                    throw new GenericRepositoryException("002", $"User {request.UserName} already exists on the Database");
                }             

                var newUser = _mapper.Map<User>(request);
                newUser.Password = _encriptionHelper.EncryptString(request.Password);

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var responseData = _mapper.Map<CreateUserResponseDTO>(newUser);

                return new GenericResponseDTO<CreateUserResponseDTO>()
                {
                    ErrorCode = "000",
                    Message = $"User {responseData.UserName} was created succesfully",
                    Data = responseData
                };

            }
            catch (SqlException exception)
            {
                return new GenericResponseDTO<CreateUserResponseDTO>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }
    
        public GenericResponseDTO<GetUserResponseDTO> GetUser(int userId)
        {
            try
            {
                var foundUser = _context.Users.FirstOrDefault(user => user.UserId == userId);

                if (foundUser == null)
                {
                    throw new GenericRepositoryException("003", $"User Id {userId} doesn't exist on the Database");
                }

                var resp = _mapper.Map<GetUserResponseDTO>(foundUser);

                return new GenericResponseDTO<GetUserResponseDTO>()
                {
                    ErrorCode = "000",
                    Message = $"User Id {userId} found",
                    Data = resp
                };

            }
            catch (SqlException exception)
            {
                return new GenericResponseDTO<GetUserResponseDTO>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }
        }

        public GenericResponseDTO<CreateUserResponseDTO> AuthenticateUser(AuthenticateUserRequestDTO request)
        {
            try
            {
                var foundUser = _context.Users.FirstOrDefault(user =>
                    user.UserName == request.UserName &&
                    user.Password == _encriptionHelper.EncryptString(request.Password));

                if (foundUser == null)
                {
                    throw new GenericRepositoryException("006", $"User {request.UserName} was not authenticated.");
                }

                var response = _mapper.Map<CreateUserResponseDTO>(foundUser);
                response.Token = _jwtHelper.CreateToken(foundUser);

                return new GenericResponseDTO<CreateUserResponseDTO>()
                {
                    ErrorCode = "000",
                    Message = $"User {foundUser.UserName} was authenticated successfully",
                    Data = response
                };
            }
            catch (SqlException exception)
            {

                return new GenericResponseDTO<CreateUserResponseDTO>()
                {
                    ErrorCode = "001",
                    Message = $"Error: {exception.Message}",
                    Data = null
                };
            }

        }
    
    }
}
