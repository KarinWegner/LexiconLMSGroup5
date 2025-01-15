namespace LMS.Blazor.Client._RoutingVariables
{
    public static class RoutingVariables
    {
        //Student CourseOverview Route variables
        public const string CourseOverview = "CourseOverview", Modules = "Modules", CourseParticipants = "CourseParticipants", Schedule = "Schedule", Planning = "Planning";
        //parameters getters
        public const string WithIntId = "{Id:int}", WithIntIdAndIntParentId = "{ParentId:int}/{Id:int}", WithStringId = "{Id}";
    }
}
