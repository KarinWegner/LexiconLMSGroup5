using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Models
{
    public class CourseEntryModel(ECourseEntryType entryType, IEnumerable<CourseEntryDto> courseEntries)
    {
        public ECourseEntryType CourseEntryType { get; set; } = entryType;
        public IEnumerable<CourseEntryDto> CourseEntries { get; set; } = courseEntries;
    }
}
