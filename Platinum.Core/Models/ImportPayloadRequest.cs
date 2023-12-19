using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Platinum.Core.Models
{
    public class ImportPayloadRequest
    {
        public IFormFile File { get; set; }
    }
}
