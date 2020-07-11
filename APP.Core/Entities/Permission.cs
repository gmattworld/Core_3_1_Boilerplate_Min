using APP.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Core.Entities
{
    /// <summary>
    /// Permission Entity
    /// </summary>
    public class Permission : BaseEntity<string>
    {
        /// <summary>
        /// Permission Code
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string Code { get; set; }

        /// <summary>
        /// Permission Name
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        /// <summary>
        /// Permission Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Permission Module
        /// </summary>
        public virtual SystemModule Module { get; set; }

        /// <summary>
        /// Require Approval check
        /// </summary>
        public bool RequireApproval { get; set; }

        /// <summary>
        /// Permission restriction status
        /// </summary>
        public bool IsRestricted { get; set; }
    }
}
