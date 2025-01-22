using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    
        public class CourseNotFoundResponse : ApiNotFoundResponse
        {
            public CourseNotFoundResponse(int id) : base($"Course with id {id} was not found")
            {
            }
        }
        public class ModuleNotFoundResponse : ApiNotFoundResponse
        {
            public ModuleNotFoundResponse(int id) : base($"Module with id {id} was not found")
            {
            }
        }
        public class ActivityNotFoundResponse : ApiNotFoundResponse
        {
            public ActivityNotFoundResponse(int id) : base($"Activity with id {id} was not found")
            {
            }
        }
        public class ActivityTypeNotFoundResponse : ApiNotFoundResponse
        {
            public ActivityTypeNotFoundResponse(int id) : base($"The Activity Type with id {id} was not found")
            {

            }
        }
        public class UserNotFoundResponse : ApiNotFoundResponse
        {
            public UserNotFoundResponse(string id) : base($"The User with id {id} was not found")
            {

            }
        }
    
}
