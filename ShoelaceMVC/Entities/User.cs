using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace $safeprojectname$.Entities
{
    public class User
    {

        //Membership required
        [Key()]
        public virtual Guid UserId { get; set; }
        [Required()]
        [MaxLength(20)]
        public virtual string Username { get; set; }
        [Required()]
        [MaxLength(250)]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required()]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual int PasswordFailuresSinceLastSuccess { get; set; }
        public virtual Nullable<DateTime> LastPasswordFailureDate { get; set; }
        public virtual string ConfirmationToken { get; set; }
        public virtual Nullable<DateTime> CreateDate { get; set; }
        public virtual Nullable<DateTime> PasswordChangedDate { get; set; }
        public virtual string PasswordVerificationToken { get; set; }
        public virtual Nullable<DateTime> PasswordVerificationTokenExpirationDate { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        //Optional
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string TimeZone { get; set; }
        public virtual string Culture { get; set; }

    }
}