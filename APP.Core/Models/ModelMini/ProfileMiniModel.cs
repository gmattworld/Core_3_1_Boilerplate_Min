using APP.Core.Enums;

namespace APP.Core.Model.ModelMini
{
	/// <summary>
	/// Profile mini model
	/// </summary>
	public class ProfileMiniModel
	{
		/// <summary>
		/// First name
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Last name
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Profession
		/// </summary>
		public string Profession { get; set; }

		/// <summary>
		/// User hobby
		/// </summary>
		public string Hobby { get; set; }

		/// <summary>
		/// User full name
		/// </summary>
		public string FullName { get; set; }

		/// <summary>
		/// User gender
		/// </summary>
		public virtual Gender Gender { get; set; }

		/// <summary>
		/// User Marital status
		/// </summary>
		public virtual MaritalStatus MaritalStatus { get; set; }

		/// <summary>
		/// User LGA
		/// </summary>
		public virtual string LGAID { get; set; }

		/// <summary>
		/// User Account
		/// </summary>
		public virtual string UserID { get; set; }
	}
}
