using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Models
{
    public class CourseEntryModel
    {
        public ECourseEntryType CourseEntryType { get; set; }
        public IEnumerable<CourseEntryDO> CourseEntries { get; set; }

        public CourseEntryModel(ECourseEntryType entryType, IEnumerable<CourseEntryDO> courseEntries)
        {
            CourseEntryType = entryType;
            CourseEntries = courseEntries;
        }      

        public CourseEntryModel(ECourseEntryType entryType)
        {
            CourseEntryType = entryType;
            CourseEntries = [];
        }
    }
}
