using System;
using System.Collections.Generic;
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


    }
}
