using Domain.Models.Entities;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using LMS.Shared.DTOs.CourseDTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class AddActivityForm
    {

        [Inject]
        private IApiService apiService { get; set; }
        [Parameter]
        public int CourseId { get; set; }
        [Parameter]
        public int ModuleId { get; set; }
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public class ActivityModel
        {



            [Required(ErrorMessage = "Activity name is required.")]
            [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
            public string Name { get; set; }
            [Required(ErrorMessage = "The Activity needs a start date")]
            public DateTime StartDate { get; set; }
            [Required(ErrorMessage = "The Activity needs an end date")]
            public DateTime EndDate { get; set; }
            [MaxLength(500, ErrorMessage = "Description can not exceed 500 characters")]
            [Required(ErrorMessage = "Activity requires a description")]
            public string Description { get; set; }
            [Required(ErrorMessage = "The Activity needs an end date")]
            public int ActivityTypeID { get; set; } = 1;

            //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            //{
            
                
            //}
        }
        [Parameter]
        public IEnumerable<ActivityTypeDTO> ActivityTypes { get; set; } = new List<ActivityTypeDTO>();
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IEnumerable<ActivityTypeDTO>? fetch = await apiService.GetAsync<IEnumerable<ActivityTypeDTO>>(VBRoutes.API.ActivityType.ActivityTypes);
                 ActivityTypes = fetch;
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
      
        private async Task HandleValidSubmit()

        {

            await OnSubmit.InvokeAsync(Model);
            await AddActivity(Model);
            navigationManager.NavigateTo(VBRoutes.Administration.LinkToCourseDashboard);

        }
        async Task AddActivity(ActivityModel model)
        {
            var ActivityCreateDto = new ActivityCreateDTO
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
                ActivityTypeId = model.ActivityTypeID,
                ModuleId= ModuleId
            };
            try
            {
                Logger.LogInformation("Attemting to create Activity!");

                var response = await apiService.PostAsync<ActivityCreateDTO, ApiResponse>(VBRoutes.API.Activity.Activities, ActivityCreateDto);

                if (response != null && response.Success)
                {
                    Logger.LogInformation("Activity was successfully created.");

                }
                else
                {
                    Logger.LogInformation("Activity creation failed.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"An error occurred while creating activity: {ex.Message}");
                
        
           
            };
            //   apiService.PostAsync<ApiResponse>(ActivityModel Model, VBRoutes.API.Cre.ActivityTypes);


        }
    }
}
