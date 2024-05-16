using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Voting.Host.Auth {

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            
            if (IsCorsPreflightRequest(Request)) {
                // Allow preflight request to proceed without authentication
                return AuthenticateResult.NoResult();
            }

            if (!Request.Headers.ContainsKey("Authorization")) {
                return AuthenticateResult.Fail("Missing Authorization header");
            }

            try {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (authHeader.Scheme != "Basic") {
                    return AuthenticateResult.Fail("Invalid authentication scheme");
                }

                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                if (!ValidateCredentials(username, password)) {
                    return AuthenticateResult.Fail("Invalid username or password");
                }

                var principal = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity(new[] {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
            }, Scheme.Name));

                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            } catch (Exception ex) {
                return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
            }
        }

        private bool ValidateCredentials(string username, string password) {
            return username == "admin" && password == "password";
        }

        private bool IsCorsPreflightRequest(HttpRequest request) {
            return request.Method == "OPTIONS" &&
                   request.Headers.ContainsKey("Origin") &&
                   request.Headers.ContainsKey("Access-Control-Request-Method");
        }
    }
}
