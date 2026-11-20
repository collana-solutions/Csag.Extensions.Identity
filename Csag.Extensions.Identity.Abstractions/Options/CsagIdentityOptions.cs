namespace Csag.Extensions.Identity.Abstractions.Options
{
    /// <summary>
    /// Csag Identity options, bound from the <c>NeolutionIdentity</c> configuration section.
    /// </summary>
    public class CsagIdentityOptions
    {
        /// <summary>
        /// Gets or sets the Google options.
        /// </summary>
        public GoogleOptions? Google { get; set; }

        /// <summary>
        /// Gets or sets the password hasher options.
        /// </summary>
        public PasswordHasherOptions PasswordHasher { get; set; } = new();
    }
}
