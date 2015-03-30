using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lab5.Models;

namespace Lab5.Services
{
    public interface IIdentityConfigurationsViewModelWriter
    {
        void EditIdentityConfigurations(IdentityConfigurationsViewModel model);
    }
}