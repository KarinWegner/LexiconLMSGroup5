using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseModulesOverview
    {

        public CourseEntryModel Entries { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Entries = new CourseEntryModel(ECourseEntryType.Module, FakeDataService.GetModules()); 
            await base.OnInitializedAsync();
        }
    }
}
