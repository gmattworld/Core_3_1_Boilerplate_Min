namespace APP.Core.Entities
{
    /// <summary>
    /// RolePermission Entity
    /// </summary>
    public class RolePermission: BaseEntity<string>
    {
        /// <summary>
        /// Get or set the Permission
        /// </summary>
        public virtual Permission ThePermission { get; set; }

        /// <summary>
        /// Get or set the user role
        /// </summary>
        public virtual Role TheRole { get; set; }
    }
}
