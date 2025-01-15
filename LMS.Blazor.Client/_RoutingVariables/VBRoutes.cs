namespace LMS.Blazor.Client._RoutingVariables
{
    //(Variable Based) Routes
    public static class VBRoutes
    {
        public record Student
        {
            public const string LinkToOverview = $"/{RoutingVariables.CourseOverview}";
            public const string LinkToModules = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.Modules}";
            public const string LinkToCourseParticipants = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.CourseParticipants}";
            public const string LinkToSchedule = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.Schedule}";
            public const string LinkToPlanning = $"{RoutingVariables.CourseOverview}/{RoutingVariables.Planning}";
            public const string LinkToSpecificModule = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.Modules}/{RoutingVariables.WithIntId}";
            public const string LinkToActivityDetails = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.Modules}/{RoutingVariables.WithIntIdAndIntParentId}";
            public const string LinkToCourseParticipantDetails = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.CourseParticipants}/{RoutingVariables.WithStringId}";
        }
    }
}
