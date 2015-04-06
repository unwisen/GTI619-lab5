using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lab5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab5.IdentityExtensions
{
    public class AppUserStore : UserStore<ApplicationUser>
    {
        public AppUserStore(DbContext context)
            : base(context)
        {
        }

        public override async Task CreateAsync(ApplicationUser appuser)
        {
            await base.CreateAsync(appuser);
            await AddToOldPasswordAsync(appuser, appuser.PasswordHash);
        }

        public Task AddToOldPasswordAsync(ApplicationUser appuser, string userpassword)
        {
            appuser.UserOldPassword.Add(new OldPassword()
            {
                UserID = appuser.Id,
                HashPassword = userpassword
            });
            return UpdateAsync(appuser);
        }
    }
}