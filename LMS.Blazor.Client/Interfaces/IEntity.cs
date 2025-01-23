namespace LMS.Blazor.Client.Interfaces
{
    public interface IEntity
    {
        string Name { get; set; }
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
        string Description { get; set; }
    }
}
