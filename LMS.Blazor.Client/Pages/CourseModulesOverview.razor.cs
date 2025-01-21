using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Linq;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseModulesOverview
    {
        [Inject]
        private IApiService apiService {  get; set; }

        public CourseEntryModel Entries { get; set; }
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
            {
                //TODO: PICK THE ACTUAL COURSE
                var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithModules(1));
                Entries = new CourseEntryModel(ECourseEntryType.Module, res.Modules.Select(x=> new CourseEntryDO(x)));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
