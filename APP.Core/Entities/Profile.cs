using APP.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
	/// <summary>
	/// User Profile Entity
	/// </summary>
	public class Profile : BaseEntity<string>
	{
		/// <summary>
		/// First name
		/// </summary>
		[Column(TypeName = "varchar(200)")]
		public string FirstName { get; set; }

		/// <summary>
		/// Last name
		/// </summary>
		[Column(TypeName = "varchar(200)")]
		public string LastName { get; set; }

		/// <summary>
		/// Profession
		/// </summary>
		[Column(TypeName = "varchar(200)")]
		public string Profession { get; set; }

		/// <summary>
		/// User hobby
		/// </summary>
		public string Hobby { get; set; }

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
		public virtual LGA TheLGA { get; set; }

		/// <summary>
		/// User Account
		/// </summary>
		public virtual User TheUser { get; set; }

		/// <summary>
		/// User full name
		/// </summary>
		[Column(TypeName = "varchar(200)")]
		public virtual string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(this.LastName) || string.IsNullOrEmpty(this.LastName.Trim()))
				{
					return string.Format("{0} ", this.FirstName);
				}
				if ((string.IsNullOrEmpty(this.LastName.Trim())) && (string.IsNullOrEmpty(this.FirstName)))
				{
					return "";
				}
				if (string.IsNullOrEmpty(this.FirstName))
				{
					return this.LastName;
				}
				return string.Format("{0}, {1}", this.LastName, this.FirstName);
			}
			set { }
		}
	}
}
