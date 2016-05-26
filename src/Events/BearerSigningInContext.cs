using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public class BearerSigningInContext : BaseBearerContext
    {
        public BearerSigningInContext(
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

        public ClaimsPrincipal Principal { get; set; }

        public AuthenticationProperties Properties { get; set; }
    }
}
