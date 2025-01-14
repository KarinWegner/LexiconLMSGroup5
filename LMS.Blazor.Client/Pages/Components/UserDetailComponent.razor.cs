using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class UserDetailComponent
    {
        [Parameter]
        public CourseParticipantDO CourseParticipant { get; set; }
    }
}
