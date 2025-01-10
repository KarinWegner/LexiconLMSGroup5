using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Models
{
    public class CourseEntryInstanceModel(ECourseEntryType entryType,CourseEntryDto courseEntry)
    {
        public ECourseEntryType CourseEntryType { get; set; } = entryType;
        public CourseEntryDto CourseEntry { get; set; } = courseEntry;
    }
}
