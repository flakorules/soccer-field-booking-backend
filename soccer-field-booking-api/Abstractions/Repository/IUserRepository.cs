using SoccerFieldBooking.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Abstractions.Repository
{
    public interface IUserRepository
    {
        Task<GenericResponseDTO<CreateUserResponseDTO>> CreateUser(CreateUserRequestDTO request);
        GenericResponseDTO<GetUserResponseDTO> GetUser(int userId);
        GenericResponseDTO<CreateUserResponseDTO> AuthenticateUser(AuthenticateUserRequestDTO request);
    }
}
