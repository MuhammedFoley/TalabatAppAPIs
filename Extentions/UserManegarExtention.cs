using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabt.Core.Entities.identity;

namespace TalabatAppAPIs.Extentions
{
    public static class UserManegarExtention
    {
        public static async Task<AppUser?> findUSerAddresAsync(this UserManager<AppUser> userManager ,ClaimsPrincipal User)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await userManager.Users.Include(U=>U.Adress).FirstOrDefaultAsync(U => U.Email==email);
            return user;
        }
    }
}
