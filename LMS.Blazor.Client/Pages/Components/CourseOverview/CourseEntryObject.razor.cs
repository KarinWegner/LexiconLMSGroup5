using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;

namespace LMS.Blazor.Client.Pages.Components.CourseOverview
{
    public partial class CourseEntryObject
    {
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        [Parameter]
        public CourseEntryDO Model { get; set; } = null!;
        [Parameter]
        public int CourseId { get; set; }
        public string ThisNavLink => CourseEntryType == ECourseEntryType.Module ?
            VBRoutes.Student.LinkToModule(CourseId, Model.Id) :
            VBRoutes.Student.LinkToActivity(CourseId, Model.ParentId, Model.Id);


    }
}
