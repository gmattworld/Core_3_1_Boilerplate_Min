namespace APP.Core.Model
{
	/// <summary>
	/// State Model
	/// </summary>
	public class StateModel : BaseModel<string>
	{
		/// <summary>
		/// State Code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// State Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// State restriction status
		/// </summary>
		public bool IsRestricted { get; set; }

		/// <summary>
		/// State Country
		/// </summary>
		public virtual string CountryId { get; set; }
	}
}
