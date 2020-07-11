namespace APP.Core.Model
{
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the primary key for this user.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Get or set the user role
        /// </summary>
        public virtual string RoleID { get; set; }
    }
}
