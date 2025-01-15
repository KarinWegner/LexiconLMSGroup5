using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Models
{
    public class CourseEntryInstanceModel(ECourseEntryType entryType,CourseEntryDO courseEntry)
    {
        public ECourseEntryType CourseEntryType { get; set; } = entryType;
        public CourseEntryDO CourseEntry { get; set; } = courseEntry;
    }
}
