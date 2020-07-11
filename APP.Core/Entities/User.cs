using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
    /// <summary>
    /// User entity
    /// </summary>
    public class User : BaseEntity<string>
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string Email { get; set; }

        /// <summary>
        /// Email Confirmed
        /// </summary>
        [Required]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string UserName { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public string SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        [Column(TypeName = "varchar(200)")]
        public string ConcurrencyStamp { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public string NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// Get or set the user role
        /// </summary>
        public virtual Role TheRole { get; set; }
    }
}
