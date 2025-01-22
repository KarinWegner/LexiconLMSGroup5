using LMS.Blazor.Client.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace LMS.Blazor.Client.Pages.Components
{
    public partial class DateWidget
    {
        [Parameter]
        public DateTime StartTime { get; set; }
        [Parameter]
        public DateTime EndTime { get; set; }
        [Parameter]
        public EDateDisplayType DisplayType { get; set; }

    }
}
