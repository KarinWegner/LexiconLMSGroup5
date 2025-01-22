using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.CourseOverview
{
    public partial class ActivityDetails
    {
        [Inject]
        private IApiService apiService { get; set; }
        [Parameter]
        public int CourseId { get; set; }
        [Parameter]
        public int ModuleId { get; set; }
        [Parameter]
        public int ActivityId { get; set; }
        public CourseEntryDO Activity = default!;


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var res = await apiService.GetAsync<ActivityDTO>(VBRoutes.API.Activity.ActivityAtId(ActivityId));
                //TODO: SHOULD NOT BE ACTIVITY
                Activity = new CourseEntryDO(res);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
