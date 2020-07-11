namespace APP.Core.Model
{
	/// <summary>
	/// Country Model
	/// </summary>
	public class CountryModel : BaseModel<string>
	{
		/// <summary>
		/// Country code
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Country Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Country restriction status
		/// </summary>
		public bool IsRestricted { get; set; }
	}
}
