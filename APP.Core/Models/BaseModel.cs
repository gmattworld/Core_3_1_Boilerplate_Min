using APP.Core.Enums;

namespace APP.Core.Model
{
	/// <summary>
	/// Base Model
	/// </summary>
	/// <typeparam name="TKey"></typeparam>
	public class BaseModel<TKey>
	{
		/// <summary>
		/// Get or set entity ID
		/// </summary>
		public virtual TKey Id { get; set; }

		/// <summary>
		/// Get or set entity record status
		/// </summary>
		public virtual RecordStatus RecordStatus { get; set; }

		/// <summary>
		/// Get or set entity record approval status
		/// </summary>
		public virtual bool IsApproved { get; set; }
	}
}
