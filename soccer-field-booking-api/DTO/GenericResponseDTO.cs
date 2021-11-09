using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.DTO
{
    public class GenericResponseDTO<T>
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }
}
