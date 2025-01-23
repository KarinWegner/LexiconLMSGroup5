namespace LMS.Blazor.Client._RoutingVariables
{
    public static class RoutingVariables
    {
        //Student CourseOverview Route variables
        public const string CourseOverview = "CourseOverview", Modules = "Modules", CourseParticipants = "CourseParticipants", Schedule = "Schedule", Planning = "Planning", AdminAccount="AdministrationAccount";
        //parameters getters
        public const string WithIntId = "{Id:int}", WithIntIdAndIntParentId = "{ParentId:int}/{Id:int}", WithStringId = "{Id}";
        //parameters getters
        public const string CourseId = "{CourseId:int}", ModuleId = "{ModuleId:int}", ActivityId = "{ActivityId:int}";
        //Adminstration route
        public const string AdministrationDashboard = "AdministrationDashboard", CourseDashboard = "CourseDashboard", StudentDashboard = "StudentDashboard";
        //API Endpoints
        public const string APIModules = "api/modules", APICourses = "api/Courses", APIActivity = "api/activities", Enrollments = "api/enrollment", EnrollmentsFromUser = "user";
        //API Course Queries
        public const string APICoursesIncludeModules = "includeModules=true", APICoursesIncludeEnrollments = "includeEnrollments=true";
        //API Module Queries
        public const string APIModulesIncludeActivities = "includeActivities=true";
        //API Enrollment Queries
        public const string APIEnrollmentsFilterByString = "?roleFilter=";
        //Acess Denied
        public const string AccessDenied = "/AccessDenied";
    }
}
