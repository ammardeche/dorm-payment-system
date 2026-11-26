using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DormPaymentSystem.Core.Rules
{
    public static class PaymentRules
    {


        public static DateTime CalculateDueDate(DateTime checkIn, int year, int month)
        {
            var dueDate = new DateTime(year, month, 15);

            

            if (checkIn.Year == year && checkIn.Month == month)
            {
                return checkIn.Date;
            }

            return dueDate;

        }


        public static decimal CalculateMonthAmount(DateTime checkIn, decimal monthlyRate, int year, int month)
        {

            return monthlyRate;

        }

    }
}