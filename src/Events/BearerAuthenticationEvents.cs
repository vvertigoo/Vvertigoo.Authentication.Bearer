using System;
using System.Threading.Tasks;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public class BearerAuthenticationEvents : IBearerAuthenticationEvents
    {
        public Func<BearerValidatePrincipalContext, Task> OnValidatePrincipal { get; set; } = context => Task.FromResult(0);

        public Func<BearerSigningInContext, Task> OnSigningIn { get; set; } = context => Task.FromResult(0);

        public Func<BearerSignedInContext, Task> OnSignedIn { get; set; } = context => Task.FromResult(0);

        public virtual Task ValidatePrincipal(BearerValidatePrincipalContext context) => OnValidatePrincipal(context);

        public virtual Task SigningIn(BearerSigningInContext context) => OnSigningIn(context);

        public virtual Task SignedIn(BearerSignedInContext context) => OnSignedIn(context);
    }
}
