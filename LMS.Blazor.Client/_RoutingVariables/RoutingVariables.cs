namespace LMS.Blazor.Client._RoutingVariables
{
    public static class RoutingVariables
    {
        //Student CourseOverview Route variables
        public const string CourseOverview = "CourseOverview", Modules = "Modules", CourseParticipants = "CourseParticipants", Schedule = "Schedule", Planning = "Planning", AdminAccount="AdministrationAccount";
        //parameters POST
        public const string CoursePost = "Courses", ModulePost = "Modules", APIPostActivity = "api/activities";
        //parameters getters
        public const string WithIntId = "{Id:int}", WithIntIdAndIntParentId = "{ParentId:int}/{Id:int}", WithStringId = "{Id}";
        //parameters getters
        public const string CourseId = "{CourseId:int}", ModuleId = "{ModuleId:int}", ActivityId = "{ActivityId:int}";
        //Adminstration route

        public const string AdministrationDashboard = "AdministrationDashboard", CourseDashboard = "CourseDashboard", StudentDashboard = "StudentDashboard", AddNewUser = "AddNewUser", DashboardModules = "Modules";
        //Administration directives
        public const string New = "new", Edit = "edit";
        //Create course/module/activity route
        public const string Create = "Create", CreateActivity = "CreateActivity", CreateModule = "CreateModule", CreateCourse = "CreateCourse";
        //API Endpoints
        public const string APIModules = "api/modules", APICourses = "api/Courses", APIActivity = "api/activities", APIActivityTypes="api/activitytypes", Enrollments = "api/enrollment", EnrollmentsFromUser = "user", ModuleCourse = "course", ModuleModule = "module", ModuleActivtity ="Activity";


        //API Course Queries
        public const string APICoursesIncludeModules = "includeModules=true", APICoursesIncludeEnrollments = "includeEnrollments=true";
        //API Module Queries
        public const string APIModulesIncludeActivities = "includeActivities=true";
        //API Enrollment Queries
        public const string APIEnrollmentsFilterByString = "?roleFilter=";
        //Access Denied
        public const string AccessDenied = "/AccessDenied";
        public const string NotFound = "/NotFound";
    }
}
