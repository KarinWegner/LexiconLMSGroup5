using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseParticipantObject
    {
        [Parameter]
        public CourseParticipantDO Model { get; set; } = null!;
        public string ThisNavLink => $"{VBRoutes.Student.LinkToCourseParticipants}/{Model.Id}";


    }
}
