using LMS.DataAccess;
using LMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LMS
{
    public class AppUserManager : UserManager<User, int>
    {
        public AppUserManager(IUserStore<User, int> store) :
            base(store)
        {
        }

        public static AppUserManager Create(IntUserStore store)
        {
            var manager = new AppUserManager(store);

            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> opt, IOwinContext ctx)
        {
            return Create(new IntUserStore(ctx.Get<LMSContext>()));
        }
    }

    public class AppSignInManager : SignInManager<User, int>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authManager) :
            base(userManager, authManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync(UserManager as AppUserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> opt, IOwinContext ctx)
        {
            return new AppSignInManager(ctx.GetUserManager<AppUserManager>(), ctx.Authentication);
        }
    }
}