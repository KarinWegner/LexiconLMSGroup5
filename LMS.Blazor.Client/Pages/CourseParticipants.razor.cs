using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseParticipants
    {
        public IEnumerable<CourseParticipantDO> Participants { get; set; }

        [Inject]
        private IApiService apiService { get; set; }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //TODO: PICK THE ACTUAL COURSE
                var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithEnrollments(1));
                Participants = res.Enrollments.Select(x => new CourseParticipantDO(x));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

    }
}
