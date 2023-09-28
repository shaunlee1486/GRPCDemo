using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
namespace GrpcServer
{
    public static class JwtAuthenticationManager
    {
        public const string JWT_TOKEN_KEY = "84322CFB66934ECC86D547C5CF4F2EFC";
        private const int JWT_TOKEN_VALIDITY = 30;
        public static AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            // Implement User Credentials Validation
            var userRole = string.Empty;
            if (request.UserName == "admin" && request.Password == "admin")
            {
                userRole = "Administrator";
            }
            else if (request.UserName == "user" && request.Password == "user")
            {
                userRole = "User";
            }
            else return null;

            //generate token
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(JWT_TOKEN_KEY);
            var tokenExpiryDateTime = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("username", request.UserName),
                    new Claim(ClaimTypes.Role, userRole)
                }),
                Expires = tokenExpiryDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature),
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);
            return new AuthenticationResponse
            {
                AccessToken = token,
                ExpiresIn = (int)tokenExpiryDateTime.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }
}
