using Amazon.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace amazon.APIs.Extentions
{
    public static class UserMangerExtention
    {
        public static async Task <AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager, ClaimsPrincipal User)
        {
            var email =User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U =>U.Address).FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
