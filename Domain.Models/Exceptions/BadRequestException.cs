using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        public string Title { get; set; }
        protected BadRequestException(string message, string title = "Bad Request") : base(message)
        {
            Title = title;
        }
    }

    public class BadDateSequenceException : BadRequestException
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public BadDateSequenceException(DateTime startDate, DateTime endDate) : base($"Start date {startDate} cannot be after end date {endDate}" )
        {
            StartDate = startDate;
            EndDate = endDate;
        } 
    }
    public class BadCourseRequestException : BadRequestException
    {
        public BadCourseRequestException(string message) : base(message) { }
    }
    
}
