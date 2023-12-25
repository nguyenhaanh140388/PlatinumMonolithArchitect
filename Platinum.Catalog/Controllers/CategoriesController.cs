using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Platinum.Catalog.Core.Abstractions.Services;
using Platinum.Catalog.Core.Features.Categories.Commands;
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
    
        }

        [HttpGet]
        [Route("get-animals")]
        public async Task<ActionResult<string>> GetListAnimal(string payload)
        {
            var result =  await this.categoryService.GetCategoriesNameQuery(payload);
            return Ok(result);
        }

        [HttpPost]
        [Route("add-category")]
        public async Task<IActionResult> AddCategory(CreateCategoryCommand payload)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await categoryService.CreateCategoryHandler(payload);
            if (!result.Succeeded)
            {
                return this.BadRequest(result);
            }

            return this.Ok(result);
        }
    }
}
