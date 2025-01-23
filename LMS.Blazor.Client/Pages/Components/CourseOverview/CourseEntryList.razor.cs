using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.CourseOverview
{
    public partial class CourseEntryList
    {
        [Parameter]
        public CourseEntryModel ListEntries { get; set; }
        [Parameter]
        public int CourseId { get; set; }
        public CourseEntryModel UpcommingEntries { get; set; }
        public CourseEntryModel ExpiredEntries { get; set; }

        protected async override Task OnParametersSetAsync()
        {
            if (ListEntries.CourseEntryType != ECourseEntryType.Class)
            {
                if (ListEntries == null)
                {
                    UpcommingEntries = new();
                    ExpiredEntries = new();
                    return;
                }
                var store = ListEntries.CourseEntries.OrderBy(x => x.EndTime);

                List<CourseEntryDO> expired = new(), next = new();

                foreach (var module in store)
                {
                    if (module.EndTime < DateTime.Now)
                        expired.Add(module);
                    else
                        next.Add(module);
                }
                UpcommingEntries = new(ListEntries.CourseEntryType, next);
                ExpiredEntries = new(ListEntries.CourseEntryType, expired);
                await base.OnParametersSetAsync();
            }
        }
    }
}
