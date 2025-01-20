using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseModulesOverview
    {
        [Inject]
        private IApiService apiService {  get; set; }

        public CourseEntryModel Entries { get; set; }

        protected async override Task OnInitializedAsync()
        {
            //Entries = new CourseEntryModel(ECourseEntryType.Module, FakeDataService.GetModules()); 
            var res = await apiService.GetAsync<IEnumerable<ModuleDTO>>(VBRoutes.API.Module.Modules);
            Entries = new CourseEntryModel(ECourseEntryType.Module, res.Select(x => new CourseEntryDO(x)).ToList());
            await base.OnInitializedAsync();
        }
    }
}
