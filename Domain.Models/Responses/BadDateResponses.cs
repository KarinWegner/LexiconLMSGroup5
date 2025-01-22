using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class BadDateSequenceResponse : ApiBadRequestResponse
    {
        public BadDateSequenceResponse(DateTime startDate, DateTime endDate) : base($"Item end date{endDate} is before its start date {startDate}.")
        {

        }
    }
    public class BadDateTimeFrameBreakResponse : ApiBadRequestResponse
    {
        public BadDateTimeFrameBreakResponse() : base("Item is scheduled outside allowed timeframe")
        {

        }
    }
    public class BadDateOverlapResponse : ApiBadRequestResponse
    {
        public BadDateOverlapResponse() : base("Another item is scheduled during entered timespan") { }
    }
}
