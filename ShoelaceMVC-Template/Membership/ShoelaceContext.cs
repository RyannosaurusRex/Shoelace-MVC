using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using $safeprojectname$.Entities;

namespace $safeprojectname$.Membership
{
    public class $safeprojectname$Context : DbContext
    {
        public $safeprojectname$Context() : base("$safeprojectname$")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
