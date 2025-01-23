using LMS.Blazor.Client._RoutingVariables;
using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs.ApplicationUserDTOs;
using LMS.Shared.DTOs.EnrollmentDTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace LMS.Blazor.Client.Pages.CourseOverview
{
    public partial class CourseMain
    {
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        IApiService ApiService { get; set; }
        [Inject]
        AuthenticationStateProvider PAuth {  get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var userData = await PAuth.GetAuthenticationStateAsync();
                var id = userData.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (id == default)
                    NavigationManager.NavigateTo(VBRoutes.AccessDenied);
                var res = await ApiService.GetAsync<IEnumerable<EnrollmentUserCourseListDTO>>(VBRoutes.API.Enrollment.GetUserEnrollment(id!.Value));
                NavigationManager.NavigateTo(VBRoutes.Student.LinkToCourse(res!.First().CourseId));
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
