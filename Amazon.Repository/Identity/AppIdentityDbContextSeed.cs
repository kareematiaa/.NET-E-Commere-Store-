using Amazon.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon.Repository.Identity
{
    public class AppIdentityDbContextSeed 
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any()) 
            {
                var user = new AppUser()
                {
                    DisplayName = "kareem Atia",
                    Email = "Kareem.Atia@gmail.com",
                    UserName = "kareem.atia",
                    PhoneNumber = "0115478888"
                };

                await userManager.CreateAsync(user,"Pa$$w0rd");
            
            }
        }
    }
}
