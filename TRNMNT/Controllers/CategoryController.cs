using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Web.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : CRUDController<Category>
    {
        public CategoryController(ILogger<CategoryController> logger, IRepository<Category> repository) : base(logger, repository)
        {
        }
    }

}
