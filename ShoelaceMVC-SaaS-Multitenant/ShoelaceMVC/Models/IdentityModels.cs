using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ShoelaceMVC.Models 
{
    // Modify the User class to add extra user information
    public class User : IUser
    {
        public User()
            : this(String.Empty)
        {
        }

        public User(string userName)
        {
            UserName = userName;
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public int AccountId { get; set; }

        public Person PersonProfile { get; set; }

        public string UserName { get; set; }
    }

    public class UserLogin : IUserLogin
    {
        [Key, Column(Order = 0)]
        public string LoginProvider { get; set; }
        [Key, Column(Order = 1)]
        public string ProviderKey { get; set; }

        public string UserId { get; set; }

        public UserLogin() { }

        public UserLogin(string userId, string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            UserId = userId;
        }
    }

    public class UserSecret : IUserSecret
    {
        public UserSecret()
        {
        }

        public UserSecret(string userName, string secret)
        {
            UserName = userName;
            Secret = secret;
        }

        [Key]
        public string UserName { get; set; }
        public string Secret { get; set; }
    }

    public class UserRole : IUserRole
    {
        [Key, Column(Order = 0)]
        public string RoleId { get; set; }
        [Key, Column(Order = 1)]
        public string UserId { get; set; }
    }

    public class Role : IRole
    {
        public Role()
            : this(String.Empty)
        {
        }

        public Role(string roleName)
        {
            Id = roleName;
        }

        [Key]
        public string Id { get; set; }
    }

    /// <summary>
    /// This is the tennant account.  This governs each instance of the application.
    /// </summary>
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Account Display Name")]
        public string Name { get; set; }

        /// <summary>
        /// The master user who purchased or own the account for all users.
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// The subdomain of the app's main URL that is associated with this account.  For usability's sake, it's best that this not be
        /// changed, since that would also require adjustment of where the VanityDomain is pointing.  I'd suggest that you put in your app for
        /// users to call you and let you manually change it, that way you can make them aware of other changes they'll have to make if they
        /// aren't technical.
        /// </summary>
        /// <example>http://contosopuppies.adoptiontools.com</example>
        public string Subdomain { get; set; }
        
        /// <summary>
        /// This is the vanity URL for tenants.  Often people want to brand their SaaS apps with their own custom domain,
        /// So this lets them specify a custom domain for their app!  They'll need to point a cname to their Subdomain for this to work, though.
        /// </summary>
        /// <example>http://contosopuppypound.com</example>
        public string VanityDomain { get; set; }
    }
}