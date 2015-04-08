using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab5.Models
{

    public class IdentityConfiguration
    {
        [Key]
        [Required]
        public int Id
        {
            get;
            set;
        }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
        public TimeSpan DefaultAccountLockoutTimeSpan { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireNonLetterOrDigit { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }
        public bool CannotReusePassword { get; set; }
    }

    public enum ConnectionStatus
    {
        [Description("A connection attempt succeed")]
        Succeed,
        [Description("A connection attempt failed")]
        Failed,
        [Description("An user modified his password")]
        ModifyPassword
    }

    public class ApplicationUserViewModel
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public int LockoutCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string HashingVersion { get; set; }
    }

    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserOldPassword = new List<OldPassword>();
            base.LockoutEnabled = true;
            HashingVersion = "1";
        }
        public virtual ICollection<OldPassword> UserOldPassword { get; set; }
        public int LockoutCount { get; set; } // Number of times the user was locked
        public string HashingVersion { get; set; } // Hashing algorithm version

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Notez qu'authenticationType doit correspondre à l'élément défini dans CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Ajouter les revendications personnalisées de l’utilisateur ici
            return userIdentity;
        }
    }

    public class OldPassword
    {
        public OldPassword()
        {
            CreatedDate = DateTimeOffset.Now;
        }
 
        [Key, Column(Order = 0)]
        public string HashPassword { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        [Key, Column(Order = 1)]
        public string UserID { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<IdentityConfiguration> IdentityConfigurations { get; set; }
    }
}