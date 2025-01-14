using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services
{
    public static class FakeDataService
    {

        public static CourseEntryDO GetCourse()
        { return new CourseEntryDO(0, 0, ".Net 2024 Q3", "Learn to do the things with the computer so the computer displays the website or whatever.", new DateTime(2024, 09, 03), new DateTime(2025, 03, 03)); }

        public static IEnumerable<CourseEntryDO> GetModules()
        {
            return [ new CourseEntryDO(0,0, "C# Basics", "Learn how to code.", new DateTime(2024,09,03,9,0,0), new DateTime(2024,10,7,17,0,0)),
                new CourseEntryDO(1,0,"Frontend", "Learn how to graphic design passion etc etc", new DateTime(2024,10,10,9,0,0), new DateTime(2024,10,28,17,0,0)),
            new CourseEntryDO(2,0,"Asp.net", "Learn how to asp", new DateTime(2024,10,28,9,0,0), new DateTime(2025,1,7,17,0,0)),
            new CourseEntryDO(3,0,"The future", "Learn how to time travel", new DateTime(2025,10,28,9,0,0), new DateTime(2026,1,7,17,0,0)),
            new CourseEntryDO(4,0,"Curveball", "Learn how to time travel", new DateTime(2025,10,28,9,0,0), new DateTime(2025,5,7,17,0,0))];
        }

        public static IEnumerable<CourseEntryDO> GetActivities()
        {
            return [ new CourseEntryDO(0,0, "C# Introduction", "Learn how to code.","Course moment", new DateTime(2024,09,03,9,0,0), new DateTime(2024,9,10,17,0,0)),
                new CourseEntryDO(1,0,"more C#", "Learn how to make a program","lecture", new DateTime(2024,9,12,9,0,0), new DateTime(2024,9,22,17,0,0)),
            new CourseEntryDO(2,0,"even more c#", "Learn how to do other stuff","assignment", new DateTime(2024,9,25,9,0,0), new DateTime(2024,10,7,17,0,0))];
        }

        public static IEnumerable<CourseParticipantDO> GetCourseParticipants()
        {
            return [new CourseParticipantDO("https://ci3.googleusercontent.com/meips/ADKq_NaBkdnx0nzVtW9QgikxlQxG8EpVt-txtIqGKRItD0wSrr2trXcjpj1EPYVIAGpZNKMTygjMSe1E5hM-vpC1FjuVeS8KoXRsVdpP5NjgtALga6Zm02CQPNi8PSS7w4C_hrezsvJ3=s0-d-e1-ft#https://du11hjcvx0uqb.cloudfront.net/dist/images/email_signature-d2c5880612.png", "student", "b", "username", "firstname lastname", "no"),
            new CourseParticipantDO(null, "student", "c", "username", "firstname lastname", "no"),
            new CourseParticipantDO(null, "student", "d", "username", "firstname lastname", "no")];
        }
    }
}
