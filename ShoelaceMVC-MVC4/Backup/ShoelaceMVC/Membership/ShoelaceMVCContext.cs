using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ShoelaceMVC.Entities;

namespace ShoelaceMVC.Membership
{
    public class ShoelaceMVCContext : DbContext
    {
        public ShoelaceMVCContext()
            : base("ShoelaceMVC")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
