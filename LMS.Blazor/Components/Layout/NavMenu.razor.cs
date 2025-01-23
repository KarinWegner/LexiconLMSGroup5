using LMS.Blazor.Client._RoutingVariables;
using Microsoft.AspNetCore.Components.Routing;

namespace LMS.Blazor.Components.Layout
{
    public partial class NavMenu
    {
        private string CourseOverviewRoute => VBRoutes.Student.LinkToMain;
        private string AdministrationDashboardLink => VBRoutes.Administration.LinkToDashboard;
        private string CourseAdministrationLink => VBRoutes.Administration.LinkToCourseDashboard;
        private string StudentAdministrationLink => VBRoutes.Administration.LinkToStudentDashboard;

        private string? currentUrl;
        private string showAdminEntries = "d-none";
        protected override void OnInitialized()
        {
            currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (currentUrl.Contains(RoutingVariables.AdministrationDashboard))
                showAdminEntries = "";
            else
                showAdminEntries = "d-none";
            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
            StateHasChanged();
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }
    }

		
	
}
