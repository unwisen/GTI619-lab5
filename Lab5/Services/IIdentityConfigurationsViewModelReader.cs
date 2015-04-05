using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Lab5.Models;

namespace Lab5.Services
{
    public interface IIdentityConfigurationsViewModelReader
    {
        Task<IdentityAndUsersConfigsViewModel> GetIdentityAndUsersConfigViewModelAsync();
        Task<IdentityConfigurationsViewModel> GetIdentityConfigViewModelAsync();
        Task<IEnumerable<ApplicationUserViewModel>> GetUsersViewModelAsync();
        ApplicationUserViewModel GetUserViewModelAsync(string id);
    }
}