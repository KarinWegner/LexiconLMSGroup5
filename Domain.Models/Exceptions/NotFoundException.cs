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
        public int Id { get; set; }
        public CourseNotFoundException(int id) : base($"The Course with id {id} was not found")
        {
            Id = id;
        }
    }
    public class ModuleNotFoundException : NotFoundException
    {
        public int Id { get; set; }
        public ModuleNotFoundException(int id) : base($"The Module with id {id} was not found")
        {
            Id = id;
        }
    }
    public class ActivityNotFoundException : NotFoundException
    {
        public int Id { get; set; }
        public ActivityNotFoundException(int id) : base($"The Activity with id {id} was not found")
        {
            Id = id;
        }
    }
    public class ActivityTypeNotFoundException : NotFoundException
    {
        public ActivityTypeNotFoundException(int id) : base($"The Activity Type with id {id} was not found")
        {

        }
    }
    public class UserNotFoundException : NotFoundException
    {
        public string UserId { get; set; }
        public UserNotFoundException(string id) : base($"The User with id {id} was not found")
        {
            UserId = id;
        }
    }
}
