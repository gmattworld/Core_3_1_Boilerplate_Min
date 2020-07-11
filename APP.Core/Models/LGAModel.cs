namespace APP.Core.Model
{
	/// <summary>
	/// LGA Model
	/// </summary>
	public class LGAModel : BaseModel<string>
	{
		/// <summary>
		/// LGA Code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// LGA Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// LGA State
		/// </summary>
		public virtual string StateId { get; set; }

		/// <summary>
		/// LGA restriction status
		/// </summary>
		public bool IsRestricted { get; set; }
	}
}
