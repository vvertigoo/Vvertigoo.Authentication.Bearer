namespace Vvertigoo.Authentication.Bearer
{
    /// <summary>
    /// Determines how the identity token's security property is set.
    /// </summary>
    public enum BearerSecureOption
    {
        /// <summary>
        /// Allow unsecure connections, default value
        /// </summary>
        Never,

        /// <summary>
        /// HTTPS only
        /// </summary>
        Always
    }
}
