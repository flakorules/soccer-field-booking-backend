﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerFieldBooking.API.Exceptions.Repository
{
    public class GenericRepositoryException:Exception
    {
        public string ErrorCode { get; }

        public GenericRepositoryException(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }
    }
}
