using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Migrations
{
    public class AppIdentityDbContextSeed
    {
        public async static Task SeedUSerAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Shubham",
                    Email = "Shubhm@gmail.com",
                    UserName = "Shubhm@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Shubham",
                        LastName = "Kumar",
                        Street = "10th Street",
                        City = "Jhunjhunu",
                        State = "Rajasthan",
                        ZipCode = "300000"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
