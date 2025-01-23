using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class AddActivityForm
    {

        [Inject]
        private IApiService apiService { get; set; }
        public class ActivityModel
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Description { get; set; }
            public  string ActivityType { get; set; }
        }
        [Parameter]
        public List<ActivityTypeDTO> ActivityTypes { get; set; } = new List<ActivityTypeDTO>();
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var activityTypes = await apiService.GetAsync<IEnumerable<ActivityTypeDTO>>(VBRoutes.API.ActivityType.ActivityTypes);
               StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        protected override async Task OnInitializedAsync()
        {
             //GetActivityTypes();
            base.OnInitialized();
            Model = new ActivityModel();
        }
        protected async Task GetActivityTypes()
        {

            var activityTypes = await apiService.GetAsync<IEnumerable<ActivityTypeDTO>>(VBRoutes.API.ActivityType.ActivityTypes);
            StateHasChanged();
        }
        

    }
}
