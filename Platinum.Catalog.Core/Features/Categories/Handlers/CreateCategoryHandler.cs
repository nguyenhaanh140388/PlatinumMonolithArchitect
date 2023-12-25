using AutoMapper;
using Platinum.Catalog.Core.Abstractions.Repositories;
using Platinum.Catalog.Core.Entities;
using Platinum.Catalog.Core.Features.Categories.Commands;
using Platinum.Core.Abstractions.Commands;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Abstractions.UnitOfWork;
using Platinum.Core.Common;
using Platinum.Core.Extensions;
using Serilog;

namespace Platinum.Catalog.Core.Features.Categories.Handlers
{
    public class CreateCategoryHandler : BaseHandler, ICommandHandlerAsync<CreateCategoryCommand, ResponseObject<Category>>
    {
        private readonly ICategoryRepository categoryRepository;

        public CreateCategoryHandler(
            ICategoryRepository categoryRepository
            , IUnitOfWork unitOfWork
            , IApplicationUserManager userManager
            , IMapper mapper
            , ILogger logger
            )
            : base(unitOfWork, userManager, mapper, logger)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<ResponseObject<Category>> HandleAsync(CreateCategoryCommand payload)
        {
            var data = mapper.Map<Category>(payload);

            await categoryRepository.InsertOrUpdateAsync(data);
            var result = await unitOfWork.SaveChangesAsync();

            return data.ToResponseObject(result > 0);
        }
    }
}
