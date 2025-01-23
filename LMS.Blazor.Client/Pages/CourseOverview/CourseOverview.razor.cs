using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.CourseOverview
{
    public partial class CourseOverview
    {
        [Inject]
        private IApiService apiService { get; set; }
        public CourseEntryDO Course { get; set; }
        [Parameter]
        public int CourseId { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtId(CourseId));
                Course = new CourseEntryDO(res!);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
