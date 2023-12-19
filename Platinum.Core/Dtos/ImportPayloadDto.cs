using Microsoft.AspNetCore.Http;

namespace Platinum.Core.Dtos
{
    public class ImportPayloadDto
    {
        public IFormFile File { get; set; }
    }
}
