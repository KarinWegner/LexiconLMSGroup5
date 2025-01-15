using Domain.Contracts;
using Domain.Models.Entities;
using LMS.Infrastructure.Data;

namespace LMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LmsContext _context;

    private IGenericRepository<Course> _courses;
    private IGenericRepository<Module> _modules;
    private IGenericRepository<Activity> _activities;
    private IGenericRepository<Document> _documents;
    private IGenericRepository<ActivityType> _activityTypes;

    public UnitOfWork(LmsContext context)
    {
        _context = context;
    }

    public IGenericRepository<Course> Courses => _courses ??= new GenericRepository<Course>(_context);
    public IGenericRepository<Module> Modules => _modules ??= new GenericRepository<Module>(_context);
    public IGenericRepository<Activity> Activities => _activities ??= new GenericRepository<Activity>(_context);
    public IGenericRepository<Document> Documents => _documents ??= new GenericRepository<Document>(_context);

    public IGenericRepository<ActivityType> ActivityTypes => _activityTypes ??= new GenericRepository<ActivityType>(_context);

    public async Task CompleteASync()
    {
        await _context.SaveChangesAsync();
    }
}
