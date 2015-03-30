using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Lab5.Models;
using Lab5.Repository;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Lab5.Services
{
    public class IdentityConfigurationsViewModelService : IIdentityConfigurationsViewModelReader, IIdentityConfigurationsViewModelWriter
    {
        private IdentityConfigurationsRepository _identityConfigurationsRepository = null;
        private ApplicationUserRepository _applicationUsersRepository = null;

        public IdentityConfigurationsViewModelService()
            : this(new IdentityConfigurationsRepository(new ApplicationDbContext()), new ApplicationUserRepository(new ApplicationDbContext())){    
        }

        public IdentityConfigurationsViewModelService(IdentityConfigurationsRepository configRepository, ApplicationUserRepository userRepository)
        {
            _identityConfigurationsRepository = configRepository;
            _applicationUsersRepository = userRepository;
        }
        public IdentityConfigurationsViewModel GetIdentityConfigurationsViewModel()
        {
            var identityConfig = _identityConfigurationsRepository.GetIdentityConfigurations().SingleOrDefault();
            var viewModel = new IdentityConfigurationsViewModel();
            if (identityConfig != null)
            {
                viewModel.IdentityConfigurationsId = identityConfig.Id;
                viewModel.MaxFailedAccessAttemptsBeforeLockout = identityConfig.MaxFailedAccessAttemptsBeforeLockout;
                viewModel.DefaultAccountLockoutTimeSpan = identityConfig.DefaultAccountLockoutTimeSpan;
                viewModel.RequireDigit = identityConfig.RequireDigit;
                viewModel.RequireLowercase = identityConfig.RequireDigit;
                viewModel.RequireNonLetterOrDigit = identityConfig.RequireNonLetterOrDigit;
                viewModel.RequireUppercase = identityConfig.RequireUppercase;
                viewModel.RequiredLength = identityConfig.RequiredLength;
            }

            var applicationUsers = _applicationUsersRepository.GetUsers().ToList();
            viewModel.ApplicationUsers = applicationUsers;

            return viewModel;
        }

        public void EditIdentityConfigurations(IdentityConfigurationsViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}