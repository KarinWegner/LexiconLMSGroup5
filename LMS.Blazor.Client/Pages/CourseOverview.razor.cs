using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseOverview
    {
        public CourseEntryDO Course { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Course = FakeDataService.GetCourse();
            await base.OnInitializedAsync();
        }
    }
}
