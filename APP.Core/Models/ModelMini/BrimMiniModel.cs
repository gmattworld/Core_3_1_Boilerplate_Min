using APP.Core.Enums;

namespace APP.Core.Model.ModelMini
{
	/// <summary>
	/// Brim Mini Model
	/// </summary>
	public class BrimMiniModel
	{
		/// <summary>
		/// Brim Title
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Brim body
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Allow comment status
		/// </summary>
		public bool AllowComment { get; set; }

		/// <summary>
		/// Brim Privacy
		/// </summary>
		public virtual Privacy Visibility { get; set; }

		/// <summary>
		/// Brim Category
		/// </summary>
		public virtual string CategoryID { get; set; }
	}
}
