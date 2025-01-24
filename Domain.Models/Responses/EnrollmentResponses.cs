using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class UserEnrollmetLimitResponse : ApiBadRequestResponse
    {        
        public UserEnrollmetLimitResponse(string userId ) : base($"User is already enrolled in a course. UserId: {userId}") { }
    }
    public class UserDublicateEnrollmentResponse : ApiBadRequestResponse
    {
        public UserDublicateEnrollmentResponse(string userId, int courseId) : base($"User is already enrolled in selected course. UserId: {userId}. CourseId: {courseId}.") { } 
    }
    public class UserMissingRoleResponse : ApiBadRequestResponse
    {
        public UserMissingRoleResponse() : base("User has not been assigned a role") { }
    }
    public class EnrollmentEditErrorResponse : ApiModificationErrorResponse 
    {
        public EnrollmentEditErrorResponse(string message) : base(message) { }
    }
    public class UserNotEnrolledResponse : ApiBadRequestResponse
    {
        public UserNotEnrolledResponse() : base("User is not enrolled in selected course") { }
    }
}
