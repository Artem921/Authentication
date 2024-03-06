
using IdentityDb.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDb
{
    public class AppIdentityContext:IdentityDbContext<AppUser,AppRole,Guid>
    {
        public AppIdentityContext(DbContextOptions<AppIdentityContext> options)
        : base(options)
        {
            Database.Migrate();
        }
     
    }
}
