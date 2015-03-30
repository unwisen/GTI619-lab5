using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab5.Models;

namespace Lab5.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            var test = context.Users.ToList();
            return context.Users;
        }
    }
}