using APP.Core.Entities;
using APP.Repository.EFRepo.EntitiesExt;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APP.Repository.EFRepo
{
    /// <summary>
    /// App DB context
    /// </summary>
    public class AppDBContext : IdentityDbContext<IUser, IRole, string>
    {
        /// <summary>
        /// MindBrimDBContext constructor
        /// </summary>
        /// <param name="options"></param>
        public AppDBContext(DbContextOptions options) : base(options) { }


        /// <summary>
        /// Country DBSet
        /// </summary>
        public DbSet<Country> Countries { get; set; }

        /// <summary>
        /// LGA DBSet
        /// </summary>
        public DbSet<LGA> LGAs { get; set; }

        /// <summary>
        /// State DBSet
        /// </summary>
        public DbSet<State> States { get; set; }

        /// <summary>
        /// Profile DBSet
        /// </summary>
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// AuditTrail DBSet
        /// </summary>
        public DbSet<AuditTrail> AuditTrails { get; set; }

        /// <summary>
        /// Permission DBSet
        /// </summary>
        public DbSet<Permission> Permissions { get; set; }

        /// <summary>
        /// RolePermission DBSet
        /// </summary>
        public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
