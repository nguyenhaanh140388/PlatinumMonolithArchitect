using Platinum.Core.Abstractions.Commands;

namespace Platinum.Core.Common
{
    public abstract class BaseService
    {
        protected readonly ICommandService commandService;

        protected BaseService(ICommandService commandService)
        {
            this.commandService = commandService;
        }
    }
}
