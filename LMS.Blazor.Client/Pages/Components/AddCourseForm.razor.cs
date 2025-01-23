using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs.CourseDTOs;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class AddCourseForm
    {
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            Model = new CourseCreateDTO();
        }
    }
}



