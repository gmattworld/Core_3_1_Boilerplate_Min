using APP.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace APP.Core.Entities
{
	/// <summary>
	/// Base entity
	/// </summary>
	/// <typeparam name="TKey">Primary Key data type</typeparam>
	public class BaseEntity<TKey>
	{
		/// <summary>
		/// Get or set entity ID
		/// </summary>
		[Required]
		[MaxLength(100)]
		public virtual TKey Id { get; set; }

		/// <summary>
		/// Get or set entity record status
		/// </summary>
		[Required]
		public virtual RecordStatus RecordStatus { get; set; }

		/// <summary>
		/// Get or set entity record approval status
		/// </summary>
		public virtual bool IsApproved { get; set; }

		/// <summary>
		/// Get or set date an entity record is created (Default to current date)
		/// </summary>
		public virtual DateTime CreatedAt { get; set; } = DateTime.Now;

		/// <summary>
		/// Get or set date an entity was last updated (Default to min date)
		/// </summary>
		public virtual DateTime ModifiedAt { get; set; } = DateTime.MinValue;
	}
}
