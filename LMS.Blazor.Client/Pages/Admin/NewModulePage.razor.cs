using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Blazor.Client.Utilities;

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
        private EditContext editContext { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        protected override void OnInitialized()
        {
            moduleCreateDTO.StartDate = DateTime.Now;
            moduleCreateDTO.EndDate = moduleCreateDTO.StartDate.AddDays(20);
            base.OnInitialized();
            editContext = new(moduleCreateDTO);
            validationMessageStore = new(editContext);
            editContext.OnFieldChanged += reset;
        }

        private void reset(object? sender, FieldChangedEventArgs e)
        {
            if (editContext.GetValidationMessages().Count() != 0)
                validationMessageStore.Clear();
        }

        public async void OnModuleSubmit(EditContext editContext)
        {
            if(editContext.GetValidationMessages().Count() != 0) 
                validationMessageStore.Clear();
            var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithModules(CourseId));
            if (moduleCreateDTO.StartDate < res.StartDate)
                validationMessageStore.Add(() => moduleCreateDTO.StartDate, $"Module Start date cannot preceed course start date of {res.StartDate}");
            if (moduleCreateDTO.EndDate > res.EndDate)
                validationMessageStore.Add(() => moduleCreateDTO.EndDate, $"Module Start date cannot exceed course end date of {res.EndDate}");
            if (res.Modules.Any(x => !(x.AreTimesSeperate(moduleCreateDTO))))
            {
                foreach ( var module in res.Modules )
                {
                   if(!module.AreTimesSeperate(moduleCreateDTO))
                    {
                        if (module.IsStartTimeOverlapping(moduleCreateDTO))
                            validationMessageStore.Add(() => moduleCreateDTO.StartDate, $"Module Start date cannot overlap with the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if(module.IsEndTimeOverlapping(moduleCreateDTO))
                            validationMessageStore.Add(() => moduleCreateDTO.EndDate, $"Module End date cannot overlap with the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if (module.IsThisFullyOverlapped(moduleCreateDTO))
                            validationMessageStore.Add(() => moduleCreateDTO.EndDate, $"Module dates cannot overlap the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if (module.IsThisFullyOverlapping(moduleCreateDTO))
                            validationMessageStore.Add(() => moduleCreateDTO.EndDate, $"Module dates cannot be overlapped by the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                    }
                }
            }
            if(editContext.GetValidationMessages().Count() == 0)
            {
                var apires = await apiService.PostAsync<ModuleCreateDTO, ApiResponse>(VBRoutes.API.Module.PostModuleAtId(CourseId), moduleCreateDTO);
                navigationManager.NavigateTo(VBRoutes.Administration.LinkToCourseDashboard);
            }
        }
    }
}
