using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Okta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthenticateController : ControllerBase
    {
        [HttpGet]
        public async Task<string> AuthenticateToken(string accessToken)
        {
            // Replace with your authorization server URL:
            var issuer = "https://dev-30574109.okta.com/oauth2/default";

            var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                issuer + "/.well-known/oauth-authorization-server",
                new OpenIdConnectConfigurationRetriever(),
                new HttpDocumentRetriever());



             var validatedToken = await ValidateToken(accessToken, issuer, configurationManager);

            if (validatedToken == null)
            {
                return "Invalid token";
            }
            else
            {
                // Additional validation...
                return "Token is valid!";
            }

           // return "You are Authorised User";
        }


        private static async Task<JwtSecurityToken> ValidateToken(
        string token,
        string issuer,
        IConfigurationManager<OpenIdConnectConfiguration> configurationManager,
        CancellationToken ct = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException(nameof(token));
            if (string.IsNullOrEmpty(issuer)) throw new ArgumentNullException(nameof(issuer));

            var discoveryDocument = await configurationManager.GetConfigurationAsync(ct);
            var signingKeys = discoveryDocument.SigningKeys;

            var validationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = true,
                RequireSignedTokens = true,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = signingKeys,
                ValidateLifetime = true,
                // Allow for some drift in server time
                // (a lower value is better; we recommend two minutes or less)
                ClockSkew = TimeSpan.FromMinutes(2),
                // See additional validation for aud below
                ValidateAudience = false,
            };

            try
            {
                var principal = new JwtSecurityTokenHandler()
                    .ValidateToken(token, validationParameters, out var rawValidatedToken);

                return (JwtSecurityToken)rawValidatedToken;
            }
            catch (SecurityTokenValidationException)
            {
                Console.WriteLine("Invalid Token");
                return null;
            }
        }

    }
}
