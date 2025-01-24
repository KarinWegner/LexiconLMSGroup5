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
        public string ThisNavLink()
        {
            switch (CourseEntryType)
            {
                case ECourseEntryType.Class:
                    return VBRoutes.Student.LinkToCourse(Model.Id);
                case ECourseEntryType.Module:
                    return VBRoutes.Student.LinkToModule(CourseId, Model.Id);
                case ECourseEntryType.Activity:
                    return VBRoutes.Student.LinkToActivity(CourseId, Model.ParentId, Model.Id);
                default:
                    return VBRoutes.AccessDenied;
            }
        }
    }
}