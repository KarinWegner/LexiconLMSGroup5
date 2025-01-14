using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseEntryList
    {
        [Parameter]
        public CourseEntryModel ListEntries { get; set; }
    }
}
