using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.Admin
{
    public partial class CourseDashboardList
    {
        [Parameter]
        public IEnumerable<CourseEntryDO> courseEntries {  get; set; }
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
    }
}
