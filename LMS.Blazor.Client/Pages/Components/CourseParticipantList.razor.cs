using LMS.Blazor.Client.Models;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseParticipantList
    {
        [Parameter]
        public IEnumerable<CourseParticipantDO> ListEntries { get; set; }

    }
}
