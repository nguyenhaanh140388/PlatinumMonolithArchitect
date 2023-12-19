using Anhny010920.Core.Domain.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Platinum.Core.Common
{
    public abstract class BaseController : Controller
    {
        protected readonly IMapper mapper;
        protected readonly ILogger logger;

        protected BaseController(IMapper mapper = null, ILogger logger = null)
        {
            this.mapper = mapper;
            this.logger = logger;
        }

        // returns the current authenticated account (null if not logged in)
        public ApplicationUser AppUserManager => (ApplicationUser)HttpContext.Items["User"];
        public List<string> UserRoles => (List<string>)HttpContext.Items["Roles"];

        protected string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        protected FileStreamResult GetFileStreamResult(Stream stream, string fileName)
        {
            return new FileStreamResult(stream, GetMIMEType(fileName))
            {
                FileDownloadName = fileName
            };
        }

        private string GetMIMEType(string fileName)
        {
            var provider =
                new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(fileName, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
