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
            foreach (var user in CourseEntry.en)
            await base.OnInitializedAsync();
        }
    }
}
