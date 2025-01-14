using LMS.Shared.DTOs;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class DetailsArticleObject
    {
        [Parameter]
        public CourseEntryDO CourseEntry { get; set; }
        [Parameter]
        public bool DisplayDate { get; set; } = false;
        [Parameter]
        public bool ShowDateOnly { get; set; } = true;
    }
}
