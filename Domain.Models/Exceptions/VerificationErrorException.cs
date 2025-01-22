using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public abstract class VerificationErrorException : Exception
    {
        public string Title { get; set; }
        protected VerificationErrorException(string message, string title = "Verification Error") : base(message)
        {
            Title = title;
        }
    }

    public class EnrollmentEditFailedException : VerificationErrorException 
    {
        public EnrollmentEditFailedException(string userId,int courseId,int newCourseId, string message): 
            base($"User was not properly removed from course. \n" +
                $"UserID: {userId}\n" +
                $"CourseID: {courseId}\n" +
                $"NewCourseID: {newCourseId}\n" +
                $"Error: {message}")
        {
            
        }
    }
   
}
