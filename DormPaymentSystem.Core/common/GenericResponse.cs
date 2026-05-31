using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DormPaymentSystem.Core.Exceptions;

namespace DormPaymentSystem.Core.common
{
    public sealed class Response<T> : Response
    {
        public T? Payload { get; }

        public Pagination? Pagination { get; }

        public Response(AppException exception) : base(exception)
        {
        }

        public Response(T? payload) : base()
        {
            Payload = payload;
        }


        public Response(T? payload, Pagination pagination)
        {
            Payload = payload;
            Pagination = pagination;
        }

    }
}