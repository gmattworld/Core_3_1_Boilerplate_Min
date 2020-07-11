namespace APP.Core.Model.ModelExt
{
	public class UsersExtModel : UserModel
	{
        /// <summary>
        /// Get or set the user role
        /// </summary>
        public virtual string RoleName { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
    }
}
