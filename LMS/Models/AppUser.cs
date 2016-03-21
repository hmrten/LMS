using LMS.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LMS.Models
{
    public class IntUserLogin : IdentityUserLogin<int> { }
    public class IntUserRole : IdentityUserRole<int> { }
    public class IntUserClaim : IdentityUserClaim<int> { }
    public class IntRole : IdentityRole<int, IntUserRole>
    {
        public IntRole() { }
        public IntRole(string name) { Name = name; }
    }

    public class IntUserStore : UserStore<AppUser, IntRole, int, IntUserLogin, IntUserRole, IntUserClaim>
    {
        public IntUserStore(LMSContext ctx) : base(ctx) { }
    }

    public class IntRoleStore : RoleStore<IntRole, int, IntUserRole>
    {
        public IntRoleStore(LMSContext ctx) : base(ctx) {}
    }

    public class AppUser : IdentityUser<int, IntUserLogin, IntUserRole, IntUserClaim>, IUser<int>
    {
        [Required, StringLength(128)]
        public string FirstName { get; set; }

        [Required, StringLength(128)]
        public string LastName { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser, int> manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}