using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
using Lab5.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Lab5.Models;

namespace Lab5
{
    // Configurer l'application que le gestionnaire des utilisateurs a utilisée dans cette application. UserManager est défini dans ASP.NET Identity et est utilisé par l'application.
    // 
    // DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    public class MyDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEf(context);
            base.Seed(context);
        }

        private void InitializeIdentityForEf(ApplicationDbContext context)
        {
            // Add the default identity configurations
            var identityConfigurations = context.IdentityConfigurations.SingleOrDefault();
            if (identityConfigurations == null)
            {
                context.IdentityConfigurations.Add(new IdentityConfiguration
                {
                    MaxFailedAccessAttemptsBeforeLockout = 2,
                    DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(1),
                    RequiredLength = 7
                });
                context.SaveChanges();
            }

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string roleName1 = "Administrateur";       

            // Create Role Administrateur if it does not exist
            if (!RoleManager.RoleExists(roleName1))
            {
                var roleresult = RoleManager.Create(new IdentityRole(roleName1));
            }

            // Create User=Administrateur with password=Administrateur
            var user1 = new ApplicationUser();
            user1.UserName = "Administrateur";
            string password1 = "Administrateur";

            var adminresult1 = UserManager.Create(user1, password1);

            // Add User Admin to role Admin
            if (adminresult1.Succeeded)
            {
                var result = UserManager.AddToRole(user1.Id, roleName1);
            }
            user1.LockoutEnabled = false;
            context.Entry(user1).State = EntityState.Modified;
            context.SaveChanges();


            string roleName2 = "Préposé au cercle";
            // Create Role « Préposé au cercle » if it does not exist
            if (!RoleManager.RoleExists(roleName2))
            {
                var roleresult = RoleManager.Create(new IdentityRole(roleName2));
            }

            // Create User=Administrateur with password=Administrateur
            var user2 = new ApplicationUser();
            user2.UserName = "Utilisateur1";
            string password2 = "Utilisateur1";

            var adminresult2 = UserManager.Create(user2, password2);

            // Add User Admin to role Admin
            if (adminresult2.Succeeded)
            {
                var result = UserManager.AddToRole(user2.Id, roleName2);
            }



            string roleName3 = "Préposé au carré";
            // Create Role « Préposé au cercle » if it does not exist
            if (!RoleManager.RoleExists(roleName3))
            {
                var roleresult = RoleManager.Create(new IdentityRole(roleName3));
            }

            // Create User=Administrateur with password=Administrateur
            var user3 = new ApplicationUser();
            user3.UserName = "Utilisateur2";
            string password3 = "Utilisateur2";

            var adminresult3 = UserManager.Create(user3, password3);

            // Add User Admin to role Admin
            if (adminresult3.Succeeded)
            {
                var result = UserManager.AddToRole(user3.Id, roleName3);
            }
        }
    }

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        /// <summary>
        ///     Returns true if the user is locked out
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override async Task<bool> IsLockedOutAsync(string username)
        {
            var user = await FindByNameAsync(username).ConfigureAwait(false);

            if (user == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "User not found",
                    username));
            }

            //var isAdministrator = Roles.FindUsersInRole("Administrateur", username).Any();
            if (user.LockoutEnabled)
            {
                if (user.LockoutCount == 2)
                {
                    return true;
                }
                if (user.LockoutCount == 1)
                {
                    var lockoutTime = user.LockoutEndDateUtc;
                    var isLocked = lockoutTime >= DateTime.UtcNow;
                    if (!isLocked && user.LockoutEndDateUtc != null)
                    {
                        user.LockoutEndDateUtc = null;
                        await Store.UpdateAsync(user);
                    }
                    return isLocked;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Increments the access failed count for the user and if the failed access account is greater than or equal
        /// to the MaxFailedAccessAttempsBeforeLockout and the user never been locked before (LockoutCount = 0), the 
        /// user will be locked out for the next DefaultAccountLockoutTimeSpan and the AccessFailedCount will be reset 
        /// to 0. This is used for locking out the user account. If it's the user already been locked before (LockoutCount = 1),
        /// lock the user indefinitively.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task AccessFailedProcessAsync(string username)
        {
            var user = await FindByNameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "User not found",
                    username));
            }
            user.AccessFailedCount++; 

            if ( user.AccessFailedCount == MaxFailedAccessAttemptsBeforeLockout)
            {
                if (user.LockoutCount == 1)
                {
                    user.LockoutCount++;
                    await ResetAccessFailedCountAsync(user.Id);

                }
                else if (user.LockoutCount == 0)
                {
                    user.LockoutCount++;
                    await ResetAccessFailedCountAsync(user.Id);

                    user.LockoutEndDateUtc = DateTime.UtcNow.Add(DefaultAccountLockoutTimeSpan);
                }
            }
            await Store.UpdateAsync(user);
        }

        public async Task<int> GetLockoutCountByName(string username)
        {
            var user = await FindByNameAsync(username).ConfigureAwait(false);
            if (user == null)
            {
                throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "User not found",
                    username));
            }
            return user.LockoutCount;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configurer la logique de validation pour les noms d'utilisateur
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };


            var identityConfigRepository = new IdentityConfigurationsRepository(new ApplicationDbContext());
            var config = identityConfigRepository.GetIdentityConfigurations().SingleOrDefault();

            // Configurer la logique de validation pour les mots de passe
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = config.RequiredLength,
                RequireNonLetterOrDigit = config.RequireNonLetterOrDigit,
                RequireDigit = config.RequireDigit,
                RequireLowercase = config.RequireLowercase,
                RequireUppercase = config.RequireUppercase,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = config.DefaultAccountLockoutTimeSpan;//TimeSpan.FromMinutes(3);
            manager.MaxFailedAccessAttemptsBeforeLockout = config.MaxFailedAccessAttemptsBeforeLockout;
                
            

            // Inscrire les fournisseurs d'authentification à 2 facteurs. Cette application utilise le téléphone et les e-mails comme procédure de réception de code pour confirmer l'utilisateur
            // Vous pouvez indiquer votre propre fournisseur et vous connecter ici.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Code de sécurité",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            
            return manager;
        }
    }

    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Indiquez votre service de messagerie ici pour envoyer un e-mail.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Indiquez votre service de sms ici pour envoyer un message texte.
            return Task.FromResult(0);
        }
    }
}
