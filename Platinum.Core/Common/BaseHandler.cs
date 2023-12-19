using AutoMapper;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Abstractions.UnitOfWork;
using Serilog;

namespace Platinum.Core.Common
{
    public abstract class BaseHandler
    {
        protected readonly IMapper mapper;
        protected readonly ILogger logger;
        protected readonly IAppUserManager userManager;
        protected readonly IUnitOfWork unitOfWork;

        protected BaseHandler(IUnitOfWork unitOfWork = null, IAppUserManager userManager = null, IMapper mapper = null, ILogger logger = null)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        //protected BaseHandler(IUnitOfWork unitOfWork, IMapper mapper = null, ILogger logger = null)
        //{
        //    this.unitOfWork = unitOfWork;
        //    this.mapper = mapper;
        //    this.logger = logger;
        //}

        //protected BaseHandler(IMapper mapper = null, ILogger logger = null)
        //{
        //    this.mapper = mapper;
        //    this.logger = logger;
        //}

        //protected BaseHandler(ILogger logger = null)
        //{
        //    this.logger = logger;
        //}
    }
}
