using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.ModuleDTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components.Admin
{
    public partial class CourseDashboardEntryBody
    {
        [Parameter]
        public CourseEntryDO CourseEntry { get; set; } = default!;
        [Parameter]
        public ECourseEntryType CourseEntryType { get; set; }
        IEnumerable<CourseEntryDO> modules = [];
        private List<string> teachers = [];
        private int nrOfStudents = 0;
        private string linkToCreateNewModule => VBRoutes.Administration.DLinkToCreateNewModule(CourseEntry.Id);
        private string linktToCreateNewActivity => VBRoutes.Administration.DLinkToCreateNewActivity(CourseEntry.ParentId, CourseEntry.Id);
        [Inject]
        private IApiService apiService {  get; set; }
        protected async override Task OnInitializedAsync()
        {
            modules = CourseEntry.CourseEntryDOs;
            if (CourseEntry.Enrollments != null && CourseEntry.Enrollments.Count() > 0)
            {
                foreach (var user in CourseEntry.Enrollments)
                { 
                    if(user.Role == "Teacher")
                        teachers.Add(user.Name!);
                    else
                        nrOfStudents++;
                }
            }
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && CourseEntryType == ECourseEntryType.Module)
            {
                modules = (await apiService.GetAsync<ModuleDTO>(VBRoutes.API.Module.ModuleAtIdWithActivities(CourseEntry.Id))).Activities.Select(x => new CourseEntryDO(x));
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
