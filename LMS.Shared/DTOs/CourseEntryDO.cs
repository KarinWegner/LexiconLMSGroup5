using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs
{
    public class CourseEntryDO
    {
        public int Id { get; set; } = -1;
        public int ParentId { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ActivityType {  get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.MaxValue;
        public IEnumerable<CourseEntryDO> CourseEntryDOs { get; set; } = [];
        public IEnumerable<ApplicationUserDTO> Enrollments { get; set; } = [];

        /// <summary>
        /// DTO Constructor Type for Activity entries
        /// </summary>
        /// <param name="id">Id for this Activity.</param>
        /// <param name="parentId">Id for the parent module.</param>
        /// <param name="name">Name of this Activity.</param>
        /// <param name="description">The Description for this Activity.</param>
        /// <param name="activityType">The Activity Type.</param>
        /// <param name="startTime">Activity start time.</param>
        /// <param name="endTime">Activity end time.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CourseEntryDO(int id, int parentId, string name, string description, string activityType, DateTime startTime, DateTime endTime)
        {
            Id = id;
            ParentId = parentId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ActivityType = activityType ?? throw new ArgumentNullException(nameof(activityType));
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// DTO Constructor Type for Module entries
        /// </summary>
        /// <param name="id">Id for this Module.</param>
        /// <param name="parentId">Id for the parent Course.</param>
        /// <param name="name">Name of this Module.</param>
        /// <param name="description">The Description for this Module.</param>
        /// <param name="startTime">Module start date.</param>
        /// <param name="endTime">Module end date.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CourseEntryDO(int id, int parentId, string name, string description, DateTime startTime, DateTime endTime)
        {
            Id = id;
            ParentId = parentId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public CourseEntryDO() { }

        public CourseEntryDO(ModuleDTO from)
        {
            Id = from.ModuleId;
            ParentId = from.CourseId;
            Name = from.Name!;
            Description = from.Description!;
            StartTime = from.StartDate;
            EndTime = from.EndDate;
            CourseEntryDOs = from.Activities.Select(x => new CourseEntryDO(x));
        }
        public CourseEntryDO(CourseDTO from)
        {
            Id = from.CourseId;
            ParentId = -1;
            Name = from.Name!;
            Description = from.Description!;
            StartTime = from.StartDate;
            EndTime = from.EndDate;
            CourseEntryDOs = from.Modules.Select(x => new CourseEntryDO(x));
            Enrollments = from.Enrollments;
        }
        public CourseEntryDO(ActivityDTO from)
        {
            Id = from.ActivityId;
            ParentId = from.ModuleId;
            Name = from.Name!;
            Description = from.Description!;
            StartTime = from.StartDate;
            EndTime = from.EndDate;
            //ActivityType = from.ActivityTypeId.ToString();
        }

        public CourseEntryDO(Domain.Models.Entities.Activity from)
        {
            Id = from.ActivityId;
            ParentId = from.ModuleId;
            Name = from.Name!;
            Description = from.Description!;
            StartTime = from.StartDate;
            EndTime = from.EndDate;
            //ActivityType = from.ActivityType.Name;
        }
    }
}
