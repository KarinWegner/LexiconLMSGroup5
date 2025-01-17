
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseDashboardContainer
    {
        private string IsMyCoursesActive = "active", IsAllCoursesActive = "";
        List<CourseEntryDO> courseEntries;
        private void setMyCoursesActive()
        {
            IsMyCoursesActive = "active";
            IsAllCoursesActive = "";
        }
        private void setAllCoursesActive()
        {
            IsMyCoursesActive = "";
            IsAllCoursesActive = "active";
        }

        protected async override Task OnInitializedAsync()
        {
            courseEntries = [FakeDataService.GetCourse()];

            await base.OnInitializedAsync();
        }
    }
}
