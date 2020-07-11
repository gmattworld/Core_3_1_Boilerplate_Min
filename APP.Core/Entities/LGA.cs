using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
	/// <summary>
	/// LGA Entity
	/// </summary>
	public class LGA : BaseEntity<string>
	{
		/// <summary>
		/// LGA Code
		/// </summary>
		[Column(TypeName = "varchar(20)")]
		public string Code { get; set; }

		/// <summary>
		/// LGA Name
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(100)")]
		public string Name { get; set; }

		/// <summary>
		/// LGA State
		/// </summary>
		public virtual State TheState { get; set; }

		/// <summary>
		/// LGA restriction status
		/// </summary>
		public bool IsRestricted { get; set; }
	}
}
