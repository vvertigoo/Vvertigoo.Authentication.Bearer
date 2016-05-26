using System.Threading.Tasks;

namespace Vvertigoo.Authentication.Bearer.Events
{
    public interface IBearerAuthenticationEvents
    {
        Task ValidatePrincipal(BearerValidatePrincipalContext context);

        Task SigningIn(BearerSigningInContext context);

        Task SignedIn(BearerSignedInContext context);
    }
}