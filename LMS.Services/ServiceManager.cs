using Services.Contracts;

namespace LMS.Services;
public class ServiceManager : IServiceManager
{

    private readonly Lazy<IAuthService> authService;
    private readonly Lazy<ICourseService> courseService;
    private readonly Lazy<IModuleService> moduleService;
    private readonly Lazy<IActivityService> activityService;
    private readonly Lazy<IDocumentService> documentService;
    private readonly Lazy<IActivityTypeService> activityTypeService;
    public IAuthService AuthService => authService.Value;

    public ICourseService CourseService => courseService.Value;

    public IModuleService ModuleService => moduleService.Value;
    public IActivityService ActivityService => activityService.Value;
    public IDocumentService DocumentService => documentService.Value;
    public IActivityTypeService ActivityTypeService => activityTypeService.Value;

    public ServiceManager(
         Lazy<IAuthService> authService,
         Lazy<ICourseService> courseService,
         Lazy<IModuleService> moduleService,
         Lazy<IActivityService> activityService,
         Lazy<IDocumentService> documentService,
         Lazy<IActivityTypeService> activityTypeService
         
         )
    {
        this.authService = authService;
        this.courseService = courseService;
        this.moduleService = moduleService;
        this.activityService = activityService;
        this.documentService = documentService;
        this.activityTypeService = activityTypeService;
    }
}
