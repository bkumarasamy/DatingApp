using System;
using System.Security.Claims;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetuserName(this ClaimsPrincipal user)
        {
            //return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetuserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}