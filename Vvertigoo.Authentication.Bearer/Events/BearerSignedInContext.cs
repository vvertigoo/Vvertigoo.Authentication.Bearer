using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Authentication;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public class BearerSignedInContext : BaseBearerContext
    {
        public BearerSignedInContext(
            HttpContext context,
            BearerAuthenticationOptions options,
            string authenticationScheme,
            ClaimsPrincipal principal,
            AuthenticationProperties properties)
            : base(context, options)
        {
            AuthenticationScheme = authenticationScheme;
            Principal = principal;
            Properties = properties;
        }

        public string AuthenticationScheme { get; }

        public ClaimsPrincipal Principal { get; }

        public AuthenticationProperties Properties { get; }
    }
}
