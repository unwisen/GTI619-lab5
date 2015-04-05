using System.Linq;
using Lab5.Models;

namespace Lab5.Repository
{
    public interface IIdentityConfigurationsRepository
    {
        IQueryable<IdentityConfiguration> GetIdentityConfigurations();
        void EditIdentityConfigurations(IdentityConfiguration identityConfiguration);

        IdentityConfiguration FindById(int p);
    }
}