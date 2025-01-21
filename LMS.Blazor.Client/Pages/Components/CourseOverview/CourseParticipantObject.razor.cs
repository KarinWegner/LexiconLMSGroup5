using LMS.Blazor.Client._RoutingVariables;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.CourseOverview
{
    public partial class CourseParticipantObject
    {
        [Parameter]
        public CourseParticipantDO Model { get; set; } = null!;
        [Parameter]
        public int CourseId { get; set; }
        public string ThisNavLink => VBRoutes.Student.LinkToCourseParticipant(CourseId, Model.Id!);


    }
}
