using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseEntryObject
    {
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        [Parameter]
        public CourseEntryDO Model { get; set; } = null!;
        public string ThisNavLink => CourseEntryType == ECourseEntryType.Module ?
            $"{VBRoutes.Student.LinkToModules}/{Model.Id}" : 
            $"{VBRoutes.Student.LinkToModules}/{Model.ParentId}/{Model.Id}";


    }
}
