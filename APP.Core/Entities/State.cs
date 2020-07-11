using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
	/// <summary>
	/// State entity
	/// </summary>
	public class State : BaseEntity<string>
	{
		/// <summary>
		/// State Code
		/// </summary>
		[Column(TypeName = "varchar(20)")]
		public string Code { get; set; }

		/// <summary>
		/// State Name
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(100)")]
		public string Name { get; set; }

		/// <summary>
		/// State Country
		/// </summary>
		public virtual Country TheCountry { get; set; }

		/// <summary>
		/// State restriction status
		/// </summary>
		public bool IsRestricted { get; set; }
	}
}
