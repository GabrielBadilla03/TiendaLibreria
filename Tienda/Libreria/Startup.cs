using Libreria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Libreria.Startup))]
namespace Libreria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            InitializeRolesAndAdminUser();
        }

        private void InitializeRolesAndAdminUser()
        {
            using (var context = new LibreriaDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("User"))
                {
                    var role = new IdentityRole("User");
                    roleManager.Create(role);
                }

                if (userManager.FindByName("ssegura30126@ufide.ac.cr") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "ssegura30126@ufide.ac.cr",
                        Email = "ssegura30126@ufide.ac.cr"
                    };

                    var adminPassword = "Sebas123?";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
                else if (userManager.FindByName("jvindas40886@ufide.ac.cr") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "jvindas40886@ufide.ac.cr",
                        Email = "jvindas40886@ufide.ac.cr"
                    };

                    var adminPassword = "Vindas123?";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
                else if (userManager.FindByName("jmiranda90400@ufide.ac.cr") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "jmiranda90400@ufide.ac.cr",
                        Email = "jmiranda90400@ufide.ac.cr"
                    };

                    var adminPassword = "Teletica123?";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
                else if (userManager.FindByName("gbadilla60348@ufide.ac.cr") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "gbadilla60348@ufide.ac.cr",
                        Email = "gbadilla60348@ufide.ac.cr"
                    };

                    var adminPassword = "Gabdi123?";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
            }
        }
    }
}
