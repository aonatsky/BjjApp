using Microsoft.Extensions.Logging;
using TRNMNT.Core.Data.Entities;
using TRNMNT.Core.Data.Repositories;

namespace TRNMNT.Controllers
{
    public class CategoryController : CRUDController<Category>
    {
        public CategoryController(ILogger<CategoryController> logger, IRepository<Category> repository):base(logger,repository)
        {            
        }
    }

}
