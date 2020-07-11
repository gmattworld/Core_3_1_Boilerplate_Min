using APP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Repository.EFRepo.EntitiesExt
{
    public class IUser : IdentityUser<string>
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(100)")]
        [PersonalData]
        public override string Id { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(150)")]
        [ProtectedPersonalData]
        public override string Email { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(200)")]
        [ProtectedPersonalData]
        public override string PasswordHash { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        [ProtectedPersonalData]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        [ProtectedPersonalData]
        public override string UserName { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public override string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public override string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public override string NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public override string NormalizedUserName { get; set; }

        /// <summary>
        /// Get or set the user role
        /// </summary>
        public virtual Role TheRole { get; set; }
    }
}
