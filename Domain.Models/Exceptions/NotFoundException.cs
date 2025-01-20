using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        public string Title { get; set; }
        protected NotFoundException(string message, string title = "Not Found") : base(message)
        {
            Title = title;
        }
    }
    public class CourseNotFoundException : NotFoundException
    {
        public CourseNotFoundException(int id) : base($"The Course with {id} is not found")
        {

        }
    }
    public class ModuleNotFoundException : NotFoundException
    {
        public ModuleNotFoundException(int id) : base($"The Module with {id} is not found")
        {

        }
    }
    public class ActivityNotFoundException : NotFoundException
    {
        public ActivityNotFoundException(int id) : base($"The Activity with {id} is not found")
        {

        }
    }
    public class ActivityTypeNotFoundException : NotFoundException
    {
        public ActivityTypeNotFoundException(int id) : base($"The Activity Type with id {id} is not found")
        {

        }
    }
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string id) : base($"The User with id {id} is not found")
        {

        }
    }
}
