using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Exceptions
{
    public class AppException : Exception
    {

        public int StatusCode { get; set; }

        protected AppException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public class NotFoundException : AppException
        {
            public NotFoundException(string message) : base(message, 404)
            {
            }
        }

        public class ConflictException : AppException
        {
            public ConflictException(string message) : base(message, 409) { }
        }

        public class ValidationException : AppException
        {
            public ValidationException(string message)
                : base(message, 422) { }
        }

        public class PaymentException : AppException
        {
            public PaymentException(string message)
                : base(message, 402) { }
        }

        public class UnauthorizedException : AppException
        {
            public UnauthorizedException(string message = "You are not authorized.")
                : base(message, 401) { }
        }

        public class ForbiddenException : AppException
        {
            public ForbiddenException(string message = "Access denied.")
                : base(message, 403) { }
        }



    }
}