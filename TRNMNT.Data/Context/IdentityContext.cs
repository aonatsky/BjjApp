using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Entities;

namespace TRNMNT.Data.Context
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext (DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
