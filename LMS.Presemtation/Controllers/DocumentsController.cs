using Domain.Models.Entities;
using LMS.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LMS.Shared.DTOs.DocumentDTOs;

namespace LMS.Presemtation.Controllers
{


    [ApiController]
    [Route("api/documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly LmsContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment; // To get to the wwwroot folder where files are saved

        public DocumentsController(LmsContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }

        // Upload a document
        [HttpPost("upload")]
        public async Task<IActionResult> UploadDocument([FromForm] DocumentUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0) return BadRequest("No file uploaded.");

            // File type and size validation
            var allowedExtensions = new[] { ".pdf", ".docx", ".png", ".jpg" };
            var fileExtension = Path.GetExtension(dto.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension)) return BadRequest("Invalid file type. Only PDF, DOCX, JPEG, and PNG are allowed.");
            const long maxFileSize = 10 * 1024 * 1024; // 10 MB ?
            if (dto.File.Length > maxFileSize) return BadRequest("File size exceeds the size limit.");

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();
            //Todo: separate logic for teacher vs student

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.File.FileName)}";
            // Generate folder path
            //Todo: send the user ID if a student is uploading
            var folderPath = GenerateFolderPath(dto.CourseId, dto.ModuleId, dto.ActivityId);
            Directory.CreateDirectory(folderPath); // Ensure the folder exists

            var fullFilePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Save to database
            var document = new Document
            {
                Name = dto.File.FileName, //storing the original name
                Description = dto.Description,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = currentUser,
                FilePath = fullFilePath,
                CourseId = dto.CourseId,
                ModuleId = dto.ModuleId,
                ActivityId = dto.ActivityId
            };
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "File uploaded successfully",
                documentId = document.Id,
                fileName = document.Name,
                downloadUrl = Url.Action("DownloadDocument", new { id = document.Id })  //generates a URL for downloading a document
            });
        }


        // Get a Document by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return NotFound(new { message = "Document not found" });

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            //ToDo: Implement restrictions on who can access what documents
            var documentDto = new DocumentDto
            {
                Id = document.Id,
                Name = document.Name,
                Description = document.Description,
                UploadedAt = document.UploadedAt,
                DownloadUrl = Url.Action("DownloadDocument", new { id = document.Id })
            };

            return Ok(documentDto);
        }


        // Get all module related documents for Students
        [HttpGet("module/{moduleId}")]
        public async Task<IActionResult> GetModuleDocuments(int moduleId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();  //Todo: Options that should be available for student, check role?

            var documents = await _context.Documents
                .Where(d => d.ModuleId == moduleId)
                .Select(d => new DocumentDto
                   {
                       Id = d.Id,
                       Name = d.Name,
                       Description = d.Description,
                       UploadedAt = d.UploadedAt,
                       DownloadUrl = Url.Action("DownloadDocument", new { id = d.Id })
                   })
                .ToListAsync();

            return Ok(documents);
        }


        //Get all activity related docs for students
        [HttpGet("activity/{activityId}")]
        public async Task<IActionResult> GetActivityDocuments(int activityId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();  //Todo: Options that should be available for student, check role?

            var documents = await _context.Documents
                .Where(d => d.ActivityId == activityId)
                .Select(d => new DocumentDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    UploadedAt = d.UploadedAt,
                    DownloadUrl = Url.Action("DownloadDocument", new { id = d.Id })
                })
                .ToListAsync();

            return Ok(documents);
        }

        // Download a document
        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null) return NotFound(new { message = "Document not found" });

            var currentUser = await _userManager.GetUserAsync(User);
            //Todo: Check for permission
            if (currentUser == null) return Forbid();

            if (!System.IO.File.Exists(document.FilePath)) return NotFound("File was not found on the server.");

            var memory = new MemoryStream();
            using (var stream = new FileStream(document.FilePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            var mimeType = GetMimeType(document.FilePath);
            return File(memory, mimeType, document.Name);
        }





        // Helper methods
        //Generate folder path like Course > Module > Activity > Submissions or Documents
        private string GenerateFolderPath(int? courseId, int? moduleId, int? activityId, int? userId = null, bool isSubmission = false)
        {
            var basePath = Path.Combine(_environment.WebRootPath, "uploads");

            if (activityId.HasValue)
            {
                // Path for activity-specific files or submissions
                var activityPath = Path.Combine(basePath, $"courses", $"course-{courseId}", $"modules", $"module-{moduleId}", $"activities", $"activity-{activityId}");

                if (isSubmission && userId != null)
                {
                    // Submissions go under the submissions folder with student-specific folders
                    return Path.Combine(activityPath, "submissions", $"student-{userId}");
                }

                // Default activity documents folder (if not a submission)
                return Path.Combine(activityPath, "documents");
            }


            if (moduleId.HasValue)
            {
                // Path for module-level documents
                return Path.Combine(basePath, $"courses", $"course-{courseId}", $"modules", $"module-{moduleId}");
            }

            if (courseId.HasValue)
            {
                // Path for course-level documents
                return Path.Combine(basePath, $"courses", $"course-{courseId}");
            }

        // Default path if no associations are provided
        return basePath;
        }


        //Helper method for file types for downloading logic
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };
        }


    }

}
