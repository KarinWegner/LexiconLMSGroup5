using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class ModuleOverview
    {
        [Parameter]
        public int Id { get; set; }
        public CourseEntryModel Entries { get; set; }
        public string ModuleName = string.Empty;
        protected async override Task OnInitializedAsync()
        {
            ModuleName = FakeDataService.GetModules().First(x => x.Id == Id).Name;
            Entries = new CourseEntryModel(ECourseEntryType.Activity, FakeDataService.GetActivities().Where(x => x.ParentId == Id));
            await base.OnInitializedAsync();
        }
    }
}
