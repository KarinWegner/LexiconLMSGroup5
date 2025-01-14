

namespace Domain.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadedAt { get; set; }
        public ApplicationUser UploadedBy { get; set; }
        public string FilePath { get; set; }
        public int? CourseId { get; set; } 
        public int? ModuleId { get; set; } 
        public int? ActivityId { get; set; }
    }
}
