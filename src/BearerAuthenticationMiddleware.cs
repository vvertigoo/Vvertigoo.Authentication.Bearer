using Microsoft.AspNetCore.Authentication;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.DataProtection;

namespace Vvertigoo.Authentication.Bearer
{
    public class BearerAuthenticationMiddleware : AuthenticationMiddleware<BearerAuthenticationOptions>
    {
        public BearerAuthenticationMiddleware(
            RequestDelegate next,
            IDataProtectionProvider dataProtectionProvider,
            IOptions<BearerAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder urlEncoder)
            : base(next, options, loggerFactory, urlEncoder)
        {
            if (dataProtectionProvider == null)
            {
                throw new ArgumentNullException(nameof(dataProtectionProvider));
            }

            if (Options.TicketDataFormat == null)
            {
                var provider = Options.DataProtectionProvider ?? dataProtectionProvider;
                var dataProtector = provider.CreateProtector(typeof(BearerAuthenticationMiddleware).FullName, Options.AuthenticationScheme, "v2");
                Options.TicketDataFormat = new TicketDataFormat(dataProtector);
            }
        }

        protected override AuthenticationHandler<BearerAuthenticationOptions> CreateHandler()
        {
            return new BearerAuthenticationHandler();
        }
    }
}
