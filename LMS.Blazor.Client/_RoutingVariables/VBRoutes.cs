namespace LMS.Blazor.Client._RoutingVariables
{
    //(Variable Based) Routes
    public static class VBRoutes
    {
        public record Student
        {
            public const string LinkToMain = $"/{RoutingVariables.CourseOverview}";
            public const string LinkToOverview = $"/{RoutingVariables.CourseOverview}/{RoutingVariables.CourseId}";
            public const string LinkToModules = $"{LinkToOverview}/{RoutingVariables.Modules}";
            public const string LinkToCourseParticipants = $"{LinkToOverview}/{RoutingVariables.CourseParticipants}";
            public const string LinkToSchedule = $"{LinkToOverview}/{RoutingVariables.Schedule}";
            public const string LinkToPlanning = $"{LinkToOverview}/{RoutingVariables.Planning}";
            public const string LinkToSpecificModule = $"{LinkToModules}/{RoutingVariables.ModuleId}";
            public const string LinkToActivityDetails = $"{LinkToSpecificModule}/{RoutingVariables.ActivityId}";
            public const string LinkToCourseParticipantDetails = $"{LinkToCourseParticipants}/{RoutingVariables.WithStringId}";
            public static string LinkToCourse(int id) => $"{LinkToMain}/{id}";
            public static string DLinkToModules(int CourseId) => $"{LinkToCourse(CourseId)}/{RoutingVariables.Modules}";
            public static string DLinkToCourseParticipants(int CourseId) => $"{LinkToCourse(CourseId)}/{RoutingVariables.CourseParticipants}";
            public static string LinkToModule(int CourseId, int ModuleId) => $"{LinkToCourse(CourseId)}/{RoutingVariables.Modules}/{ModuleId}";
            public static string LinkToActivity(int CourseId, int ModuleId, int ActivityId) => $"{LinkToModule(CourseId, ModuleId)}/{ActivityId}";
            public static string LinkToCourseParticipant(int CourseId, string id) => $"{DLinkToCourseParticipants(CourseId)}/{id}";
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
            public record Enrollment
            {
                public const string Enrollments = RoutingVariables.Enrollments;
                public const string EnrollmentsUser = $"{Enrollments}/{RoutingVariables.EnrollmentsFromUser}";
                public static string GetUserEnrollment(string id) => $"{EnrollmentsUser}/{id}";
                public static string GetUserEnrollmentWithFiler(string filter) => $"{EnrollmentsUser}{RoutingVariables.APIEnrollmentsFilterByString}{filter}";
            }
        }
        public const string AccessDenied = RoutingVariables.AccessDenied;
    }
}
