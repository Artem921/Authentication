using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityDb.Entities
{
    public class AppUser:IdentityUser<Guid>
    {
        public AppUser() { }
        public AppUser(string email):base(email) { }
    }
}
