using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.ModuleDTOs;
using System.Linq;

namespace LMS.Blazor.Client.Pages.CourseOverview
{
    public partial class ModuleOverview
    {
        [Inject]
        private IApiService apiService { get; set; }
        [Parameter]
        public int CourseId { get; set; }
        [Parameter]
        public int ModuleId { get; set; }
        public CourseEntryModel Entries { get; set; }
        public string ModuleName = string.Empty;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var res = await apiService.GetAsync<ModuleDTO>(VBRoutes.API.Module.ModuleAtIdWithActivities(ModuleId));
                ModuleName = res.Name;
                //TODO: SHOULD NOT BE ACTIVITY
                Entries = new CourseEntryModel(ECourseEntryType.Activity, res.Activities.Select(x => new CourseEntryDO(x)));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
