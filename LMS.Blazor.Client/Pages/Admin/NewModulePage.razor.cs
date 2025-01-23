using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.CourseDTOs;

namespace LMS.Blazor.Client.Pages.Admin
{
    public partial class NewModulePage
    {
        [Parameter]
        public int CourseId { get; set; }
        public ModuleCreateDTO moduleCreateDTO { get; set; } = new();
        [Inject]
        private IApiService apiService { get; set; }
        EventCallback<ModuleCreateDTO> eventCallback { get; set; }
        private ValidationMessageStore validationMessageStore { get; set; }
        protected override void OnInitialized()
        {
            moduleCreateDTO.StartDate = DateTime.Now;
            moduleCreateDTO.EndDate = moduleCreateDTO.StartDate.AddDays(20);
            eventCallback = new EventCallback<ModuleCreateDTO>();
            base.OnInitialized();
        }
        public async void OnModuleSubmit(EditContext editContext)
        {
            validationMessageStore = new(editContext);
            var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithModules(CourseId));
            if (moduleCreateDTO.StartDate < res.StartDate)
                validationMessageStore.Add(() => moduleCreateDTO.StartDate, $"Module Start date cannot preceed course start date of {res.StartDate}");
            if (moduleCreateDTO.EndDate > res.EndDate)
                validationMessageStore.Add(() => moduleCreateDTO.EndDate, $"Module Start date cannot exceed course end date of {res.EndDate}");
            if(res.Modules.Any(x => x.StartDate)
        }
    }
}
