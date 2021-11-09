using AutoMapper;
using SoccerFieldBooking.API.Persistency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Repository
{

    public class RepositoryBase
    { 
        protected SoccerFieldBookingDBContext _context;
        protected IMapper _mapper;

        public RepositoryBase(SoccerFieldBookingDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        
        }
    }
}
