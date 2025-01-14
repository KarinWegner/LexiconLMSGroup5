using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages
{
    public partial class UserDetails
    {
        [Parameter]
        public string Id { get; set; }
        public CourseParticipantDO CourseParticipant { get; set; }
        protected async override Task OnInitializedAsync()
        {
            CourseParticipant = FakeDataService.GetCourseParticipants().First(x => x.Id == Id);
            await base.OnInitializedAsync();
        }
    }
}
