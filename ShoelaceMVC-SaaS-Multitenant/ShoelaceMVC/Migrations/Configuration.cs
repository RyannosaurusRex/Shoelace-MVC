namespace ShoelaceMVC.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using ShoelaceMVC.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoelaceMVC.Models.ShoelaceDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "ShoelaceMVC.Models.IdentityDbContext";
        }

#if DEBUG
        protected override void Seed(ShoelaceMVC.Models.ShoelaceDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            var dbContextCreator = new DbContextFactory<ShoelaceDbContext>();
            var Secrets = new EFUserSecretStore<UserSecret>(dbContextCreator);
            var Logins = new EFUserLoginStore<UserLogin>(dbContextCreator);
            var Users = new EFUserStore<User>(dbContextCreator);
            var Roles = new EFRoleStore<Role, UserRole>(dbContextCreator);
            var userName = "admin";
            User user = new User(userName);
            Users.Create(user);
            Roles.AddUserToRole("Admin", userName);
            //**********************************************************************************************/
            // TODO: Change the admin password from "password" to something else RIGHT NOW!
            // Sketchy hacker dudes are totally gonna guess that!
            //**********************************************************************************************/
            Secrets.Create(new UserSecret(userName, "password"));  //<<<================
            Logins.Add(new UserLogin(user.Id, IdentityConfig.LocalLoginProvider, userName));

            var admin = context.Users.First(x => x.UserName == userName);
            // "localhost" as the domain will allow you to run the app locally and test using the demo account.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Accounts.Add(new ShoelaceMVC.Models.Account() { Name = "Demo", Owner = admin, Subdomain = "demo", VanityDomain = "localhost"}); 
        }
#endif
    }
}
