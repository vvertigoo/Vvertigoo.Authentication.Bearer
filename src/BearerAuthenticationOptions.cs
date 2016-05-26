using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using Vvertigoo.Authentication.Bearer.Events;

namespace Vvertigoo.Authentication.Bearer
{
    /// <summary>
    /// Configuration options for <see cref="BearerAuthenticationMiddleware"/>.
    /// </summary>
    public class BearerAuthenticationOptions : AuthenticationOptions, IOptions<BearerAuthenticationOptions>
    {
        /// <summary>
        /// Create an instance of the options initialized with the default values
        /// </summary>
        public BearerAuthenticationOptions()
        {
            AuthenticationScheme = BearerAuthenticationDefaults.AuthenticationScheme;
            BearerSecure = BearerSecureOption.Never;
            Events = new BearerAuthenticationEvents();
            ExpireTimeSpan = TimeSpan.FromDays(14);
            AutomaticAuthenticate = true;
        }

        /// <summary>
        /// Controls how much time the token will remain valid from the point it is created. The expiration
        /// information is in the protected token ticket. Because of that an expired token will be ignored 
        /// even if it is passed to the server after the browser should have purged it 
        /// </summary>
        public TimeSpan ExpireTimeSpan { get; set; }

        /// <summary>
        /// Determines if the token should only be transmitted on HTTPS request.
        /// </summary>
        public BearerSecureOption BearerSecure { get; set; }

        /// <summary>
        /// If set this will be used by the <see cref="BearerAuthenticationMiddleware"/> for data protection.
        /// </summary>
        public IDataProtectionProvider DataProtectionProvider { get; set; }

        /// <summary>
        /// The TicketDataFormat is used to protect and unprotect the identity and other properties which are stored in the
        /// token. If it is not provided a default data handler is created using the data protection service contained
        /// in the IApplicationBuilder.Properties. The default data protection service is based on machine key when running on ASP.NET, 
        /// and on DPAPI when running in a different process.
        /// </summary>
        public ISecureDataFormat<AuthenticationTicket> TicketDataFormat { get; set; }

        /// <summary>
        /// The Provider may be assigned to an instance of an object created by the application at startup time. The middleware
        /// calls methods on the provider which give the application control at certain points where processing is occurring. 
        /// If it is not provided a default instance is supplied which does nothing when the methods are called.
        /// </summary>
        public IBearerAuthenticationEvents Events { get; set; }

        BearerAuthenticationOptions IOptions<BearerAuthenticationOptions>.Value
        {
            get
            {
                return this;
            }
        }
    }
}
