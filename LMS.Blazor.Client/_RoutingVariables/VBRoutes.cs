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
            public static string LinkToModule(int id) => $"{LinkToModules}/{id}";
            public static string LinkToActivity(int parentId, int id) => $"{LinkToModule(parentId)}/{id}";
            public static string LinkToCourseParticipant(string id) => $"{LinkToCourseParticipants}/{id}";

        }

        public record Administration
        {
            public const string LinkToDashboard = $"/{RoutingVariables.AdministrationDashboard}";
            public const string LinkToCourseDashboard = $"/{RoutingVariables.AdministrationDashboard}/{RoutingVariables.CourseDashboard}";
            public const string LinkToStudentDashboard = $"/{RoutingVariables.AdministrationDashboard}/{RoutingVariables.StudentDashboard}";
        }

        public record API
        {
            public record Module
            {
                public const string Modules = RoutingVariables.APIModules;
                public static string ModuleAtIdWithActivities(int id) => $"{RoutingVariables.APIModules}/{id}?{RoutingVariables.APIModulesIncludeActivities}";

            }
            public record Course
            {
                public const string Courses = RoutingVariables.APICourses;
                public const string CoursesWithModulesAndEnrollments = $"{RoutingVariables.APICourses}?{RoutingVariables.APICoursesIncludeModules}&{RoutingVariables.APICoursesIncludeEnrollments}";
                public static string CourseAtIdWithModules (int id) => $"{RoutingVariables.APICourses}/{id}?{RoutingVariables.APICoursesIncludeModules}";
                public static string CourseAtIdWithEnrollments(int id) => $"{RoutingVariables.APICourses}/{id}?{RoutingVariables.APICoursesIncludeEnrollments}";

                public static string CourseAtId(int id) => $"{RoutingVariables.APICourses}/{id}";
            }
            public record Activity
            {
                public const string Activities = RoutingVariables.APIActivity;
                public static string ActivityAtId(int id) => $"{RoutingVariables.APIActivity}/{id}";
            }
        }
    }
}
