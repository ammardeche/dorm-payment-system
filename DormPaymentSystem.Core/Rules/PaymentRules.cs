using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Rules
{
    public static class PaymentRules
    {


        public static DateTime CalculateDueDate(int year, int month)
        {
            return new DateTime(year, month, 15);
        }


        public static decimal CalculateMonthAmount(DateTime checkIn, decimal monthlyRate, int year, int month)
        {

            return monthlyRate;

        }

    }
}