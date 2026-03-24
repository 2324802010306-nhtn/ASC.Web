using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ASC.Web; // 👈 QUAN TRỌNG

namespace ASC.Web
{
    public interface IIdentitySeed
    {
        Task Seed(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<ApplicationSettings> options
        );
    }
}