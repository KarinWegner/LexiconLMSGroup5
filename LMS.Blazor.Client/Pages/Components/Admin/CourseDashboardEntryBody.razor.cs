using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.Admin
{
    public partial class CourseDashboardEntryBody
    {
        [Parameter]
        public CourseEntryDO CourseEntry { get; set; } = default!;
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        IEnumerable<CourseEntryDO> modules = [];
        private List<string> teachers = [];
        private int nrOfStudents = 0;
        protected async override Task OnInitializedAsync()
        {
            modules = CourseEntry.CourseEntryDOs;
            if (CourseEntry.Enrollments != null && CourseEntry.Enrollments.Count() > 0)
            {
                foreach (var user in CourseEntry.Enrollments)
                { 
                    if(user.Role == "Teacher")
                        teachers.Add(user.Name!);
                    else
                        nrOfStudents++;
                }
            }
            await base.OnInitializedAsync();
        }
    }
}
