using LMS.Blazor.Client.Models.Enums;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;
using Microsoft.JSInterop;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class CourseNavbar
    {
        [Parameter]
        public ECourseNav ActiveNav { get; set; } = ECourseNav.Overview;
        public string LinkToOverview => VBRoutes.Student.LinkToOverview;
        public string LinkToModules => VBRoutes.Student.LinkToModules;
        public string LinkToCourseParticipants => VBRoutes.Student.LinkToCourseParticipants;
        public string LinkToSchedule => VBRoutes.Student.LinkToSchedule;
        public string LinkToPlanning => VBRoutes.Student.LinkToPlanning;

        private bool isOpen = true;
        private string navstate = "collapse", NavstateBg = "bg-expanded", filler = "col", onInfo ="";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (ActiveNav == ECourseNav.Overview)
                onInfo = "active";
        }

        public void ToggleNavbar()
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                navstate = "collapse";
                NavstateBg = "bg-expanded";
                filler = "col";
            }
            else
            {
                navstate = "";
                NavstateBg = "bg-opened";
                filler = "";
            }
            
        }


    }
}