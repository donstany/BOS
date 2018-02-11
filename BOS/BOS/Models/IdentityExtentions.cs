using BOS.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.Linq;
using System.Web.Mvc;

namespace Bos.Models
{
    public static class IdentityExtensions

    {
        public static string GetUserFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetUserBirthday(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("BirthDay");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        
    }
}
