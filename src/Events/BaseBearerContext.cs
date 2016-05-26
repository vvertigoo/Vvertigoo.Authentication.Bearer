using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public class BaseBearerContext : BaseContext
    {
        public BaseBearerContext(
            HttpContext context,
            BearerAuthenticationOptions options)
            : base(context)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options;
        }

        public BearerAuthenticationOptions Options { get; }
    }
}
