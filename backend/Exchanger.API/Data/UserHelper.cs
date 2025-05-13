using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Exchanger.API.Data
{
    public static class UserHelper
    {
        public static string GetCurrentUserId(HttpContext context)
        {
            var userId = context.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Uanuthorized");
            }

            return userId;
        }
    }
}
