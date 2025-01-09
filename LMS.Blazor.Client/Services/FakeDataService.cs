using LMS.Shared.DTOs;

namespace LMS.Blazor.Client.Services
{
    public static class FakeDataService
    {

        public static IEnumerable<CourseEntryDto> GetModules()
        {
            return [ new CourseEntryDto(0,0, "C# Basics", "Learn how to code.", new DateTime(2024,09,03,9,0,0), new DateTime(2024,10,7,17,0,0)),
                new CourseEntryDto(1,0,"Frontend", "Learn how to graphic design passion etc etc", new DateTime(2024,10,10,9,0,0), new DateTime(2024,10,28,17,0,0)),
            new CourseEntryDto(2,0,"Asp.net", "Learn how to asp", new DateTime(2024,10,28,9,0,0), new DateTime(2025,1,7,17,0,0))];
        }

        public static IEnumerable<CourseEntryDto> GetActivities()
        {
            return [ new CourseEntryDto(0,0, "C# Introduction", "Learn how to code.","Course moment", new DateTime(2024,09,03,9,0,0), new DateTime(2024,9,10,17,0,0)),
                new CourseEntryDto(1,0,"more C#", "Learn how to make a program","lecture", new DateTime(2024,9,12,9,0,0), new DateTime(2024,9,22,17,0,0)),
            new CourseEntryDto(2,0,"even more c#", "Learn how to do other stuff","assignment", new DateTime(2024,9,25,9,0,0), new DateTime(2024,10,7,17,0,0))];
        }

    }
}
