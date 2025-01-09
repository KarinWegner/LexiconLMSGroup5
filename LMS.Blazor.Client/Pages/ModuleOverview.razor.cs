using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages
{
    public partial class ModuleOverview
    {
        [Parameter]
        public int Id { get; set; }
        public CourseEntryModel Entries { get; set; }
        protected async override Task OnInitializedAsync()
        {
            Entries = new CourseEntryModel(ECourseEntryType.Activity, FakeDataService.GetActivities());
            await base.OnInitializedAsync();
        }
    }
}
