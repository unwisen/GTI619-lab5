using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Lab5.Models;

namespace Lab5.Repository
{
    public class IdentityConfigurationsRepository : BaseRepository<IdentityConfiguration>, IIdentityConfigurationsRepository
    {
        public IdentityConfigurationsRepository(ApplicationDbContext context) : base(context)
        {
            
        }
        public IQueryable<IdentityConfiguration> GetIdentityConfigurations()
        {
            return context.IdentityConfigurations.AsQueryable();
        }

        public void EditIdentityConfigurations(IdentityConfiguration identityConfiguration)
        {
            context.Entry(identityConfiguration).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IdentityConfiguration FindById(int id)
        {
            return( context.IdentityConfigurations.SingleOrDefault(config => config.Id == id) );
        }
    }
}