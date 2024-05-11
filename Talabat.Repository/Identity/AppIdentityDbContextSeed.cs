using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(AppIdentityDbContext context)
        {
           
            if (!context.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Joe",
                    Email = "yousagshoieb@gmail.com"
                };
                await context.Users.AddAsync(user);
            }
            
        }
    }
}
