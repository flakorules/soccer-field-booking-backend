using AutoMapper;
using SoccerFieldBooking.API.DTO;
using SoccerFieldBooking.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateUserRequestDTO, User>();
            CreateMap<User, CreateUserResponseDTO>();
            CreateMap<User, GetUserResponseDTO>();
            CreateMap<User, AuthenticateUserResponseDTO>();
        }
    }
}
