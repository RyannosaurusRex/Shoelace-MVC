using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ShoelaceMVC.Entities;

namespace ShoelaceMVC.Membership
{
    public class ShoelaceContext : DbContext
    {
        public ShoelaceContext() : base("ShoelaceMVC")
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
