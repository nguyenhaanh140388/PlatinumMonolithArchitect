using Microsoft.AspNetCore.Http;

namespace Platinum.Core.Models
{
    public class ImportPayloadRequest
    {
        public IFormFile File { get; set; }
    }
}
