using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;

namespace LMS.Blazor.Client.Pages
{
    public partial class UserDetails
    {
        [Parameter]
        public string Id { get; set; }
        public CourseParticipantDO CourseParticipant { get; set; } = default!;
        [Inject]
        private IApiService apiService { get; set; }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //TODO: PICK THE ACTUAL COURSE
                var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithEnrollments(1));
                CourseParticipant = new CourseParticipantDO(res.Enrollments.First(u => u.Id == Id));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
