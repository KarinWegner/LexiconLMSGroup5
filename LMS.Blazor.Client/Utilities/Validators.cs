using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ModuleDTOs;

namespace LMS.Blazor.Client.Utilities
{
    public static class Validators
    {
        public static bool IsValid<T>(this IEnumerable<T> data)
        {
            if (data != null && data.Count() != 0)
                return true;
            return false;
        }
        public static bool IsValid(this CourseEntryModel model)
        {
            if (model == null) return false;
            if (model.CourseEntries == null) return false;
            if (model.CourseEntries.Count() == 0) return false;
            return true;
        }
        public static bool AreTimesSeperate(this ITimeDTO MD, ITimeDTO MCD)
        {
            if (MD.EndDate > MCD.StartDate && MD.StartDate < MCD.StartDate)
                return false;
            if (MD.StartDate < MCD.EndDate && MD.EndDate > MCD.EndDate)
                return false;
            if(MD.EndDate < MCD.EndDate && MD.StartDate > MCD.StartDate) 
                return false;
            if (MD.EndDate > MCD.EndDate && MD.StartDate < MCD.StartDate) 
                return false;

            return true;

        }
        public static bool IsStartTimeOverlapping(this ITimeDTO MD, ITimeDTO MCD)
        {
            if (MD.EndDate > MCD.StartDate && MD.StartDate < MCD.StartDate)
                return true;
            return false;
        }
        public static bool IsEndTimeOverlapping(this ITimeDTO MD, ITimeDTO MCD)
        {
            if (MD.StartDate < MCD.EndDate && MD.EndDate > MCD.EndDate)
                return true;
            return false;
        }
        public static bool IsThisFullyOverlapped(this ITimeDTO MD, ITimeDTO MCD)
        {
            if (MD.EndDate < MCD.EndDate && MD.StartDate > MCD.StartDate)
                return true;
            return false;
        }
        public static bool IsThisFullyOverlapping(this ITimeDTO MD, ITimeDTO MCD)
        {
            if (MD.EndDate > MCD.EndDate && MD.StartDate < MCD.StartDate)
                return true;
            return false;
        }
    }
}
