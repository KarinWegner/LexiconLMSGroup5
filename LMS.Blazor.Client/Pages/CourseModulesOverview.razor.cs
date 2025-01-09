using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseModulesOverview
    {

        public CourseEntryModel UpcommingEntries { get; set; }
        public CourseEntryModel ExpiredEntries { get; set; }

        protected async override Task OnInitializedAsync()
        {
            var store = FakeDataService.GetModules().OrderBy(x => x.EndTime); 
             
            List<CourseEntryDto> expired = new(), next = new();
            foreach (var module in store)
            {
                if(module.EndTime < DateTime.Now)
                    expired.Add(module);
                else
                    next.Add(module);
            }
            UpcommingEntries = new(ECourseEntryType.Module, next);
            ExpiredEntries = new(ECourseEntryType.Module, expired);
            await base.OnInitializedAsync();
        }
    }
}
