using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PGManagerApi.Services;

namespace PGManagerApi.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        UserService UserService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            UserService userService
            )
            : base(options, logger, encoder, clock)
        {
            this.UserService = userService;
        }
    
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Response.Headers.Add("WWW-Authenticate", "Basic");
    
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization header missing."));
            }
    
            // Get authorization key
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authHeaderRegex = new Regex(@"Basic (.*)");
    
            if (!authHeaderRegex.IsMatch(authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Authorization code not formatted properly."));
            }
    
            var authBase64 = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderRegex.Replace(authorizationHeader, "$1")));
            var authSplit = authBase64.Split(Convert.ToChar(":"), 2);
            var authUsername = authSplit[0];
            var authPassword = authSplit.Length > 1 ? authSplit[1] : throw new Exception("Unable to get password");
    
            var dbUser = this.UserService.Get(authUsername);
            if (dbUser == null || !BCrypt.Net.BCrypt.Verify(authPassword, dbUser.PasswordHash))
            {
                return Task.FromResult(AuthenticateResult.Fail("The username or password is not correct."));
            }
    
            var authenticatedUser = new AuthenticatedUser("BasicAuthentication", true, dbUser.Name);
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(authenticatedUser));
    
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
        }
    }
}