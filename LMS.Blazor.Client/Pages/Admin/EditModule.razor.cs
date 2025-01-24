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
    public partial class EditModule
    {
        [Parameter]
        public int CourseId { get; set; }
        [Parameter]
        public int ModuleId { get; set; }
        public ModuleUpdateDTO moduleUpdateDTO { get; set; } = new();
        [Inject]
        private IApiService apiService { get; set; }
        private ValidationMessageStore validationMessageStore { get; set; }
        private EditContext editContext { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }
        private ModuleUpdateDTO updateDTO { get; set; }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            editContext = new(moduleUpdateDTO);
            validationMessageStore = new(editContext);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                updateDTO = new ModuleUpdateDTO(await apiService.GetAsync<ModuleDTO>(VBRoutes.API.Module.ModuleAtId(ModuleId)));
                moduleUpdateDTO = new ModuleUpdateDTO(updateDTO);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async void OnModuleSubmit(EditContext editContext)
        {
            if (editContext.GetValidationMessages().Count() != 0)
                validationMessageStore.Clear();
            var res = await apiService.GetAsync<CourseDTO>(VBRoutes.API.Course.CourseAtIdWithModules(CourseId));
            if (moduleUpdateDTO.StartDate < res.StartDate)
                validationMessageStore.Add(() => moduleUpdateDTO.StartDate, $"Module Start date cannot preceed course start date of {res.StartDate}");
            if (moduleUpdateDTO.EndDate > res.EndDate)
                validationMessageStore.Add(() => moduleUpdateDTO.EndDate, $"Module Start date cannot exceed course end date of {res.EndDate}");
            if (res.Modules.Any(x => !(x.AreTimesSeperate(moduleUpdateDTO))))
            {
                foreach (var module in res.Modules)
                {
                    if (!module.AreTimesSeperate(moduleUpdateDTO))
                    {
                        if (module.IsStartTimeOverlapping(moduleUpdateDTO))
                            validationMessageStore.Add(() => moduleUpdateDTO.StartDate, $"Module Start date cannot overlap with the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if (module.IsEndTimeOverlapping(moduleUpdateDTO))
                            validationMessageStore.Add(() => moduleUpdateDTO.EndDate, $"Module End date cannot overlap with the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if (module.IsThisFullyOverlapped(moduleUpdateDTO))
                            validationMessageStore.Add(() => moduleUpdateDTO.EndDate, $"Module dates cannot overlap the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                        if (module.IsThisFullyOverlapping(moduleUpdateDTO))
                            validationMessageStore.Add(() => moduleUpdateDTO.EndDate, $"Module dates cannot be overlapped by the {module.Name} module datespan of {module.StartDate}-{module.EndDate}");
                    }
                }
            }
            if (editContext.GetValidationMessages().Count() == 0)
            {
                updateDTO = new ModuleUpdateDTO(moduleUpdateDTO, updateDTO);
                var apires = await apiService.PutAsync<ModuleUpdateDTO, ApiResponse>(VBRoutes.API.Module.PutModuleAtId(CourseId, ModuleId), updateDTO);
                navigationManager.NavigateTo(VBRoutes.Administration.LinkToCourseDashboard);
            }
        }
    }
}
