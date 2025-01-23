using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs.CourseDTOs;
using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs.ActivityDTOs;
using LMS.Shared.DTOs.ActivityTypeDTOs;
using LMS.Shared.DTOs;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class AddCourseForm
    {
        [Inject]
        private IApiService apiService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        public class CourseModel
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
        }

        
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            Model = new CourseModel();
        }

        private async Task HandleValidSubmit()
        {
            await OnSubmit.InvokeAsync(Model);
            await AddCourse(Model);
            NavigationManager.NavigateTo(VBRoutes.Administration.LinkToCourseDashboard);
        }
        async Task AddCourse(CourseModel model)
        {
            var CourseCreateDTO = new CourseCreateDTO()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Description = model.Description,
            };

            try
            {
               // Logger.LogInformation("Attemting to create Activity!");

                var response = await apiService.PostAsync<CourseCreateDTO, ApiResponse>($"api/courses", CourseCreateDTO);

                if (response != null && response.Success)
                {
                    // Logger.LogInformation("Activity was successfully created.");
                    
                }
                else
                {
                   //  Logger.LogInformation("Activity creation failed.");
                }
            }
            catch (Exception ex)
            {
               // Logger.LogError($"An error occurred while creating activity: {ex.Message}");



            };

            //   apiService.PostAsync<ApiResponse>(ActivityModel Model, VBRoutes.API.Cre.ActivityTypes);


        }
    }
}



