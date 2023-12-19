using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Platinum.Catalog.Core.Abstractions.Services;
using Platinum.Core.Common;
using Serilog;

namespace Platinum.Catalog.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService,
            IMapper mapper,
            ILogger logger
            )
            : base(mapper, logger)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        [Route("get-animals")]
        public async Task<ActionResult<string>> GetListAnimal(string payload)
        {
            var result =  await this.categoryService.GetCategoriesNameQuery(payload);
            return Ok(result);
        }
    }
}
