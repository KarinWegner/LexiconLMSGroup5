
using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.Admin
{
    public partial class CourseDashboardContainer
    {
        private string IsMyCoursesActive = "active", IsAllCoursesActive = "";
        List<CourseEntryDO> courseEntries;
        [Inject]
        private IApiService apiService { get; set; }
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

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var res = await apiService.GetAsync<(IEnumerable<CourseDTO> courseDtos, int totalCount)>(VBRoutes.API.Course.CoursesWithModulesAndEnrollments);
                courseEntries = res.courseDtos.Select(x => new CourseEntryDO(x)).ToList();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
