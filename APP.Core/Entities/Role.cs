using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
    /// <summary>
    /// Role entity
    /// </summary>
    public class Role : BaseEntity<string>
    {
        /// <summary>
        /// Get or set description for this role
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
		[Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the normalized name for this role.
        /// </summary>
		[Column(TypeName = "varchar(100)")]
        public string NormalizedName { get; set; }

        /// <summary>
        /// A random value that should change whenever a role is persisted to the store
        /// </summary>
		[Column(TypeName = "varchar(200)")]
        public string ConcurrencyStamp { get; set; }

        /// <summary>
		/// Role permissions
		/// </summary>
		public virtual IEnumerable<RolePermission> RolePermissions { get; set; }
    }
}
