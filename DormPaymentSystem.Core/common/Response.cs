using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Exceptions;

namespace DormPaymentSystem.Core.common
{
    public class Response
    {
        public bool Success { get; }

        public AppException? Exception { get; }

        public Response()
        {
            Success = true;
        }

        public Response(AppException exception)
        {
            Success = false;
            Exception = exception;
        }
    }
}