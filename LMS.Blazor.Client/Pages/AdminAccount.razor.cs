using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using LMS.Blazor.Client._RoutingVariables;

namespace LMS.Blazor.Client.Pages
{
    public partial class AdminAccount
    {
        [Inject]
        private IApiService _apiService { get; set; } = default!;
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        private const string Student = "Student";
        private const string Teacher = "Teacher";

        private string searchTerm = string.Empty;
        private string activeTab = Student;
        private IEnumerable<ApplicationUserListDTO> data = [];
        private IEnumerable<ApplicationUserListDTO> searchResults = [];
        private bool isLoading = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadAndSetUsersAsync();
            }
        }

        private async Task<IEnumerable<ApplicationUserListDTO>> LoadUsersAsync(string roleFilter)
        {
            return (await _apiService.GetAsync<(IEnumerable<ApplicationUserListDTO> userList, int totalCount)>(VBRoutes.API.Enrollment.GetUserEnrollmentWithFiler(roleFilter))).userList;
        }

        private async Task LoadAndSetUsersAsync()
        {
            isLoading = true;
            StateHasChanged();

            var loadedData = await LoadUsersAsync(activeTab);

            data = loadedData;
            searchResults = data;
            isLoading = false;
            StateHasChanged();
        }
        private async void SwitchTab(string tabName)
        {
            if (activeTab != tabName)
            {
                activeTab = tabName;
                await LoadAndSetUsersAsync();
            }
        }

        private void HandleSearch(string searchString)
        {
            searchTerm = searchString;

            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchResults = data;
            }
            else
            {
                searchResults = data
                    .Where(user => user.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            StateHasChanged();
        }
    }
}
