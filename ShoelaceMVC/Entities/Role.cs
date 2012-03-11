using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$.Entities
{
    public class Role
    {
        //Membership required
        [Key()]
        public virtual Guid RoleId { get; set; }
        [Required()]
        [MaxLength(100)]
        public virtual string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        //Optional
        [MaxLength(250)]
        public virtual string Description { get; set; }
    }
}