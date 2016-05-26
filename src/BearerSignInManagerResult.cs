namespace Vvertigoo.Authentication.Bearer
{
    /// <summary>
    /// Result of sign in process in <see cref="BearerSignInManager{TUser}"/>
    /// </summary>
    public enum BearerSignInManagerResult
    {
        /// <summary>
        /// Everything OK, user signed in
        /// </summary>
        Success,

        /// <summary>
        /// User's password is wrong
        /// </summary>
        WrongPassword,

        /// <summary>
        /// Something happened while processing sign in
        /// </summary>
        InternalError
    }
}
