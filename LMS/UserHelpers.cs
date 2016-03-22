using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace LMS
{
    public static class UserHelpers
    {
        public static IEnumerable<string> GetRoles()
        {
            return (HttpContext.Current.GetOwinContext().Request.User.Identity as ClaimsIdentity)
                .Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
        }
    }
}