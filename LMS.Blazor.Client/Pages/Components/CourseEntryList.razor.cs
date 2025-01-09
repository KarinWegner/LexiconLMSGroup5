using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseEntryList
    {
        [Parameter]
        public CourseEntryDto[] ListEntries { get; set; }
    }
}
