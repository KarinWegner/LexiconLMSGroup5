using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Shared.DTOs.DocumentDTOs
{
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime UploadedAt { get; set; }
        public string DownloadUrl { get; set; }
    }
}
