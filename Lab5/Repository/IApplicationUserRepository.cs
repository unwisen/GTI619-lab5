using System.Linq;
using Lab5.Models;

namespace Lab5.Repository
{
    public interface IApplicationUserRepository
    {
        IQueryable<ApplicationUser> GetUsers();
        ApplicationUser FindById(string id);
        void EditUserConfigurations(ApplicationUser applicationUser);
    }
}