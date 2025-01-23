using LMS.Blazor.Client._RoutingVariables;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.CourseOverview
{
    public partial class CourseMain
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }
        protected async override Task OnInitializedAsync()
        {
            //TODO: SET ACTUAL USER ID
            NavigationManager.NavigateTo(VBRoutes.Student.LinkToCourse(1030));
            await base.OnInitializedAsync();

        }
    }
}
