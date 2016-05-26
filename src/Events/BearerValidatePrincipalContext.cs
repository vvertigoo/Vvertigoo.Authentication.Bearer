using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System;
using System.Security.Claims;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public class BearerValidatePrincipalContext : BaseBearerContext
    {
        public BearerValidatePrincipalContext(HttpContext context, AuthenticationTicket ticket, BearerAuthenticationOptions options)
            : base(context, options)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Principal = ticket.Principal;
            Properties = ticket.Properties;
        }

        public ClaimsPrincipal Principal { get; private set; }

        public AuthenticationProperties Properties { get; private set; }

        public void ReplacePrincipal(ClaimsPrincipal principal)
        {
            Principal = principal;
        }

        public void RejectPrincipal()
        {
            Principal = null;
        }
    }
}
