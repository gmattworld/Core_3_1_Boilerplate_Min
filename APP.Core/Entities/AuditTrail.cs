using APP.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
    public class AuditTrail : BaseEntity<string>
    {
        /// <summary>
		/// User Account
		/// </summary>
		public virtual User TheUser { get; set; }

        /// <summary>
        /// User Browser
        /// </summary>
		[Column(TypeName = "varchar(200)")]
        public string Browser { get; set; }

        /// <summary>
        /// User IP Address
        /// </summary>
		[MaxLength(30)]
        public string IPAddress { get; set; }

        /// <summary>
        /// User Activity
        /// </summary>
		[Column(TypeName = "varchar(200)")]
        public string Activity { get; set; }

        /// <summary>
        /// User Activity Type
        /// </summary>
        public ActivityType ActivityType { get; set; }

        /// <summary>
        /// Activity Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Data after
        /// </summary>
        public string DataBefore { get; set; }

        /// <summary>
        /// Data before
        /// </summary>
        public string DataAfter { get; set; }
    }
}
