using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;

namespace LMS.Blazor.Client.Pages.Components
{
    using RV = RoutingVariables;
    public partial class CourseEntryObject
    {
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        [Parameter]
        public CourseEntryDto Model { get; set; } = null!;
        public string ThisNavLink => CourseEntryType == ECourseEntryType.Module ?
            $"{RV.CourseOverview}/{RV.Modules}/{Model.Id}" : 
            $"{RV.CourseOverview}/{RV.Modules}/{Model.ParentId}/{Model.Id}";


    }
}
