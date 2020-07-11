using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
	/// <summary>
	/// Country entity
	/// </summary>
	public class Country: BaseEntity<string>
    {
		/// <summary>
		/// Country code
		/// </summary>
		[Column(TypeName = "varchar(20)")]
		public string Code { get; set; }

		/// <summary>
		/// Country Name
		/// </summary>
		[Required]
		[Column(TypeName = "varchar(100)")]
		public string Name { get; set; }

		/// <summary>
		/// Country restriction status
		/// </summary>
		public bool IsRestricted { get; set; }
	}
}
