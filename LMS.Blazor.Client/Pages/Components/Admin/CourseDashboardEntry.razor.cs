using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace LMS.Blazor.Client.Pages.Components.Admin
{
    public partial class CourseDashboardEntry
    {
        [Parameter]
        public CourseEntryDO CourseEntry { get; set; }
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        private bool isExpanded = false;
        private void toggleExpanded()
        {
            isExpanded = !isExpanded;
        }
    }
}