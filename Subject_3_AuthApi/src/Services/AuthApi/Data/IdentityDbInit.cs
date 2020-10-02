using AuthApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AuthApi.Data
{
    public class IdentityDbInit
    {
        public static async Task Initialize(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            context.Database.Migrate();
            if (context.Users.Any(r => r.UserName == "test@test.com"))
            {
                return;
            }
                

            string user = "test@test.com";
            string password = "P@ssword1";
            await userManager.CreateAsync(new ApplicationUser { UserName = user, EmailConfirmed = true }, password);
        }
    }
}
