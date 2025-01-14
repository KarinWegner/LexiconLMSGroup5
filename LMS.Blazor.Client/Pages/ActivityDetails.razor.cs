using LMS.Blazor.Client.Models.Enums;
using LMS.Blazor.Client.Models;
using LMS.Blazor.Client.Services;
using Microsoft.AspNetCore.Components;
using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Pages
{
    public partial class ActivityDetails
    {
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        public int ParentId { get; set; }
        public CourseEntryDO Activity = default!;
        protected async override Task OnInitializedAsync()
        {
            Activity = FakeDataService.GetActivities().Where(x => x.ParentId == ParentId).First(x => x.Id == Id);
            
            await base.OnInitializedAsync();
        }
    }
}
