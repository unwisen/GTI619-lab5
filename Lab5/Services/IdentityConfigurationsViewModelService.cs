using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Lab5.Models;
using Lab5.Repository;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab5.Services
{
    public class IdentityConfigurationsViewModelService : IIdentityConfigurationsViewModelReader, IIdentityConfigurationsViewModelWriter
    {
        private IIdentityConfigurationsRepository _identityConfigurationsRepository = null;
        private IApplicationUserRepository _applicationUsersRepository = null;

        public IdentityConfigurationsViewModelService()
            : this(new IdentityConfigurationsRepository(new ApplicationDbContext()), new ApplicationUserRepository(new ApplicationDbContext())){    
        }

        public IdentityConfigurationsViewModelService(IdentityConfigurationsRepository configRepository, ApplicationUserRepository userRepository)
        {
            _identityConfigurationsRepository = configRepository;
            _applicationUsersRepository = userRepository;
        }

        public async Task<IdentityAndUsersConfigsViewModel> GetIdentityAndUsersConfigViewModelAsync()
        {
            var viewModel = new IdentityAndUsersConfigsViewModel
            {
                IdentityConfigurations = await GetIdentityConfigViewModelAsync(),
                ApplicationUsers = await GetUsersViewModelAsync()
            };

            return viewModel;
        }

        public async Task<IdentityConfigurationsViewModel> GetIdentityConfigViewModelAsync()
        {
            //PRE-CONDITION : there is one and only one identity configurations entity
            // it's because of the bad architecture.
            
            // Get entities
            var identityConfigTask = await _identityConfigurationsRepository.GetIdentityConfigurations().SingleOrDefaultAsync();
            

            // Map entities to the view model
            var model = new IdentityConfigurationsViewModel()
            {
                ID = identityConfigTask.Id,
                DefaultAccountLockoutTimeSpan = identityConfigTask.DefaultAccountLockoutTimeSpan.Minutes,
                MaxFailedAccessAttemptsBeforeLockout = identityConfigTask.MaxFailedAccessAttemptsBeforeLockout,
                RequireDigit = identityConfigTask.RequireDigit,
                RequireLowercase = identityConfigTask.RequireLowercase,
                RequireNonLetterOrDigit = identityConfigTask.RequireNonLetterOrDigit,
                RequireUppercase = identityConfigTask.RequireUppercase,
                RequiredLength = identityConfigTask.RequiredLength,
                CannotReusePassword = identityConfigTask.CannotReusePassword
            };
            return model;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetUsersViewModelAsync()
        {
            // Get users entities
            var users = await _applicationUsersRepository.GetUsers().ToListAsync();

            // Map entities to view model
            var model =
                users.Select(user => new ApplicationUserViewModel
                {
                    ID = user.Id,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutCount = user.LockoutCount,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    AccessFailedCount = user.AccessFailedCount,
                    UserName = user.UserName,
                    HashingVersion = user.HashingVersion
                });

            return model;
        }

        public ApplicationUserViewModel GetUserViewModelAsync(string id)
        {
            if (id == null)
            {
                throw new InstanceNotFoundException(String.Format(CultureInfo.CurrentCulture, "id is null", ""));
            }
            // Get user entity
            var user = _applicationUsersRepository.FindById(id);
            
            // Map entity to view model
            var model = new ApplicationUserViewModel
                {
                    ID = user.Id,
                    LockoutEnabled = user.LockoutEnabled,
                    LockoutCount = user.LockoutCount,
                    LockoutEndDateUtc = user.LockoutEndDateUtc,
                    AccessFailedCount = user.AccessFailedCount,
                    UserName = user.UserName,
                    HashingVersion = user.HashingVersion
                };

            return model;
        }

        public void EditIdentityConfigurations(IdentityConfigurationsViewModel model)
        {
            // get the entities
            var identityConfigurations = _identityConfigurationsRepository.FindById(model.ID);
            if (identityConfigurations == null)
            {
                throw new InstanceNotFoundException(String.Format(CultureInfo.CurrentCulture, "Identity configuration entity not found",
                    ""));
            }

            // map the entities with the model
            identityConfigurations.Id = model.ID;
            identityConfigurations.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(model.DefaultAccountLockoutTimeSpan);
            identityConfigurations.MaxFailedAccessAttemptsBeforeLockout = model.MaxFailedAccessAttemptsBeforeLockout;
            identityConfigurations.RequireDigit = model.RequireDigit;
            identityConfigurations.RequireLowercase = model.RequireLowercase;
            identityConfigurations.RequireNonLetterOrDigit = model.RequireNonLetterOrDigit;
            identityConfigurations.RequireUppercase = model.RequireUppercase;
            identityConfigurations.RequiredLength = model.RequiredLength;
            identityConfigurations.CannotReusePassword = model.CannotReusePassword;
            
            _identityConfigurationsRepository.EditIdentityConfigurations(identityConfigurations);
        }

        public void EditUserConfigurations(ApplicationUserViewModel model)
        {
            // Get entity
            var user = _applicationUsersRepository.FindById(model.ID);
            if (user == null)
            {
                throw new InstanceNotFoundException(String.Format(CultureInfo.CurrentCulture, "User entity not found",
                    ""));
            }
            user.LockoutEnabled = model.LockoutEnabled;
            _applicationUsersRepository.EditUserConfigurations(user);
        }
    }
}