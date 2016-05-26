using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using System.Linq;
using Vvertigoo.Authentication.Bearer.Events;
using System.IO;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace Vvertigoo.Authentication.Bearer
{
    internal class BearerAuthenticationHandler : AuthenticationHandler<BearerAuthenticationOptions>
    {
        private Task<AuthenticateResult> _readHeaderTask;
        private DateTimeOffset? _refreshIssuedUtc;
        private DateTimeOffset? _refreshExpiresUtc;
        private Task<AuthenticateResult> EnsureHeaderTicket()
        {
            if (_readHeaderTask == null)
            {
                _readHeaderTask = ReadHeaderTicket();
            }
            return _readHeaderTask;
        }

        private async Task<AuthenticateResult> ReadHeaderTicket()
        {
            if (!Context.Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Skip();
            var headerValue = Context.Request.Headers["Authorization"].First();

            var ticket = Options.TicketDataFormat.Unprotect(headerValue, GetTlsTokenBinding());
            if (ticket == null)
            {
                return AuthenticateResult.Fail("Unprotect ticket failed");
            }

            var currentUtc = DateTimeOffset.UtcNow;
            var issuedUtc = ticket.Properties.IssuedUtc;
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc != null && expiresUtc.Value < currentUtc)
            {
                return AuthenticateResult.Fail("Ticket expired");
            }

            return AuthenticateResult.Success(ticket);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var result = await EnsureHeaderTicket();
            if (!result.Succeeded)
            {
                return result;
            }

            var context = new BearerValidatePrincipalContext(Context, result.Ticket, Options);
            await Options.Events.ValidatePrincipal(context);

            if (context.Principal == null)
            {
                return AuthenticateResult.Fail("No principal.");
            }

            return AuthenticateResult.Success(new AuthenticationTicket(context.Principal, context.Properties, Options.AuthenticationScheme));
        }

        private void ApplyResponse(AuthenticationTicket ticket, string token)
        {
            if (!Request.IsHttps && Options.BearerSecure == BearerSecureOption.Always)
            {
                Response.StatusCode = 500;
                Response.Headers["Cache-Control"] = "no-cache";
                Response.Headers["Expires"] = "-1";
                Response.Headers["Pragma"] = "no-cache";
                return;
            }
            string response = CreateJsonResponse(ticket, token);

            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["Expires"] = "-1";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["X-Auth-Object"] = response;
        }

        private static string CreateJsonResponse(AuthenticationTicket ticket, string token)
        {
            var expires_in = ticket.Properties.ExpiresUtc.Value - ticket.Properties.IssuedUtc.Value;
            return "{\"issued\":\""
                + ticket.Properties.IssuedUtc.Value.ToString("r")
                + "\",\"expires\":\""
                + ticket.Properties.ExpiresUtc.Value.ToString("r")
                + "\",\"expires_in\":\""
                + expires_in.TotalSeconds
                + "\",\"access_token\":\""
                + token
                + "\"}";
        }

        protected override async Task HandleSignInAsync(SignInContext signin)
        {
            var result = await EnsureHeaderTicket();

            var signInContext = new BearerSigningInContext(
                Context,
                Options,
                Options.AuthenticationScheme,
                signin.Principal,
                new AuthenticationProperties(signin.Properties));

            DateTimeOffset issuedUtc;
            if (signInContext.Properties.IssuedUtc.HasValue)
            {
                issuedUtc = signInContext.Properties.IssuedUtc.Value;
            }
            else
            {
                issuedUtc = DateTimeOffset.UtcNow;
                signInContext.Properties.IssuedUtc = issuedUtc;
            }

            if (!signInContext.Properties.ExpiresUtc.HasValue)
            {
                signInContext.Properties.ExpiresUtc = issuedUtc.Add(Options.ExpireTimeSpan);
            }

            await Options.Events.SigningIn(signInContext);

            var ticket = new AuthenticationTicket(signInContext.Principal, signInContext.Properties, signInContext.AuthenticationScheme);
            var token = Options.TicketDataFormat.Protect(ticket, GetTlsTokenBinding());

            ApplyResponse(ticket, token);

            var signedInContext = new BearerSignedInContext(
                Context,
                Options,
                Options.AuthenticationScheme,
                signInContext.Principal,
                signInContext.Properties);

            await Options.Events.SignedIn(signedInContext);
        }

        private string GetTlsTokenBinding()
        {
            var binding = Context.Features.Get<ITlsTokenBindingFeature>()?.GetProvidedTokenBindingId();
            return binding == null ? null : Convert.ToBase64String(binding);
        }
    }
}