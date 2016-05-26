using System;
using Microsoft.Extensions.Options;
using Vvertigoo.Authentication.Bearer;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods to add bearer authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class BearerAppBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="BearerAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables bearer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<BearerAuthenticationMiddleware>();
        }

        /// <summary>
        /// Adds the <see cref="BearerAuthenticationMiddleware"/> middleware to the specified <see cref="IApplicationBuilder"/>, which enables bearer authentication capabilities.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> to add the middleware to.</param>
        /// <param name="options">A <see cref="BearerAuthenticationOptions"/> that specifies options for the middleware.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseBearerAuthentication(this IApplicationBuilder app, BearerAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<BearerAuthenticationMiddleware>(Options.Create(options));
        }
    }
}
