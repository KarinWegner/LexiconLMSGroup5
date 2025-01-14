using Domain.Models.Entities;

namespace Domain.Contracts;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Module> Modules { get; }
    IGenericRepository<Activity> Activities { get; }
    IGenericRepository<Document> Documents { get; }
    IGenericRepository<ActivityType> ActivityTypes { get; }
    Task CompleteASync();
}