using Microsoft.AspNetCore.Mvc;
using Platinum.Core.Common;

namespace Platinum.Catalog.Controllers
{
    public class CategoriesController : BaseController
    {
        [HttpGet]
        [Route("get-animals")]
        public ActionResult<List<string>> GetListAnimal()
        {
            return Ok(new List<string>() { "dog", "cat", "fish" });

        }
    }
}
