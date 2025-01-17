using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseDashboardEntryBody
    {
        [Parameter]
        public CourseEntryDO CourseEntry { get; set; }
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        IEnumerable<CourseEntryDO> modules;
        protected async override Task OnInitializedAsync()
        {
            if(CourseEntryType == ECourseEntryType.Class)
                modules = FakeDataService.GetModules();
            await base.OnInitializedAsync();
        }
    }
}
