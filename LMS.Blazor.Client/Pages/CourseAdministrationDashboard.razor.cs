using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseAdministrationDashboard
    {
        [Inject]
        private IApiService apiService {  get; set; }

        protected async override Task OnInitializedAsync()
        {
            var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CoursesWithModulesAndEnrollments);
            await base.OnInitializedAsync();
        }
    }
}
