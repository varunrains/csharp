using System.Security.Claims;
using System.Security.Principal;

namespace IplServerSide.Helpers
{
    public static class UserHelper
    {
        public static int GetUserId(IIdentity identity)
        {
            var claimsIdentity = (ClaimsIdentity) identity;
            return int.Parse(claimsIdentity.FindFirst(ClaimTypes.Sid).Value);
        }

        public static string GetUserRole(IIdentity identity)
        {
            var claimsIdentity = (ClaimsIdentity)identity;
            return claimsIdentity.FindFirst(ClaimTypes.Role).Value;
        }

    }
}