namespace Voting.Host.Auth {
    public class BasicAuthenticationOptions {
        public Func<string, string, bool>? CredentialValidator { get; set; }
    }
}
