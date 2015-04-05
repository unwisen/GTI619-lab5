using System.Data.Entity;
using System.Linq;
using Lab5.Models;

namespace Lab5.Repository
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<ApplicationUser> GetUsers()
        {
            return context.Users.AsQueryable();
        }

        public ApplicationUser FindById(string id)
        {
            return context.Users.Find(id);
        }

        public void EditUserConfigurations(ApplicationUser applicationUser)
        {
            context.Entry(applicationUser).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}