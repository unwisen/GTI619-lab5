using System.Collections.Generic;

namespace Lab5.Models
{
    public class IdentityConfigurationsViewModel
    {
        public int IdentityConfigurationsId { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }
        public int DefaultAccountLockoutTimeSpan { get; set; }
        public int RequiredLength { get; set; }
        public bool RequireNonLetterOrDigit { get; set; }
        public bool RequireDigit { get; set; }
        public bool RequireLowercase { get; set; }
        public bool RequireUppercase { get; set; }

        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
    }
}