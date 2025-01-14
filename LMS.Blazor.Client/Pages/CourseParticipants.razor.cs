using LMS.Blazor.Client.Services;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class CourseParticipants
    {
        public IEnumerable<CourseParticipantDO> Participants { get; set; }

        protected async override Task OnInitializedAsync()
        {

            Participants = FakeDataService.GetCourseParticipants();

            await base.OnInitializedAsync();
        }

    }
}
