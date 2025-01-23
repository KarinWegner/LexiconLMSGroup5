using LMS.Blazor.Client.Models;

namespace LMS.Blazor.Client.Utilities
{
    public static class Validators
    {
        public static bool IsValid<T>(this IEnumerable<T> data)
        {
            if(data != null && data.Count() != 0)
                return true;
            return false;
        }
        public static bool IsValid(this CourseEntryModel model)
        {
            if(model == null) return false;
            if(model.CourseEntries == null) return false;
            if(model.CourseEntries.Count() == 0) return false;
            return true;
        }

    }
}
