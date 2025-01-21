using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs.ActivityDTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class ActivityDetails
    {
        [Inject]
        private IApiService apiService { get; set; }
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public int ParentId { get; set; }
        public CourseEntryDO Activity = default!;
        //protected async override Task OnInitializedAsync()
        //{
        //    Activity = FakeDataService.GetActivities().Where(x => x.ParentId == ParentId).First(x => x.Id == Id);
            
        //    await base.OnInitializedAsync();
        //}
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var res = await apiService.GetAsync<ActivityDTO>(VBRoutes.API.Activity.ActivityAtId(Id));
                //TODO: SHOULD NOT BE ACTIVITY
                Activity = new CourseEntryDO(res);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
