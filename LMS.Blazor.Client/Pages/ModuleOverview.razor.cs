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
        public CourseEntryModel UpcommingEntries { get; set; }
        public CourseEntryModel ExpiredEntries { get; set; }

        public string ModuleName = string.Empty;
        protected async override Task OnInitializedAsync()
        {
            ModuleName = FakeDataService.GetModules().First(x => x.Id == Id).Name;
            var store = FakeDataService.GetActivities().OrderBy(x => x.EndTime); 

            List<CourseEntryDto> expired = new(), next = new();
            foreach (var module in store)
            {
                if (module.EndTime < DateTime.Now)
                    expired.Add(module);
                else
                    next.Add(module);
            }
            UpcommingEntries = new(ECourseEntryType.Activity, next);
            ExpiredEntries = new(ECourseEntryType.Activity, expired);
            await base.OnInitializedAsync();
        }
    }
}
