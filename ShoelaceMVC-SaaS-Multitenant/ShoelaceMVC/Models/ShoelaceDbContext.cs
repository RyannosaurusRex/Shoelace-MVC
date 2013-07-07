using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace ShoelaceMVC.Models
{
    public class ShoelaceDbContext : DbContext
    {
        public ShoelaceDbContext()
            : base("DefaultConnection")
        {
        }

        public ShoelaceDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        /// <summary>
        /// This method ensures that user names are always unique
        /// </summary>
        /// <param name="entityEntry"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            if (entityEntry.State == EntityState.Added)
            {
                User user = entityEntry.Entity as User;
                // Check for uniqueness of user name
                if (user != null && Users.Where(u => u.UserName.ToUpper() == user.UserName.ToUpper()).Count() > 0)
                {
                    var result = new DbEntityValidationResult(entityEntry, new List<DbValidationError>());
                    result.ValidationErrors.Add(new DbValidationError("User", "User name must be unique."));
                    return result;
                }
            }
            return base.ValidateEntity(entityEntry, items);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSecret> Secrets { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Person> People { get; set; }
    }
}