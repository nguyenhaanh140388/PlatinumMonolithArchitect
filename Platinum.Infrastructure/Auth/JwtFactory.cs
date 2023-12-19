using Microsoft.Extensions.Options;
using Platinum.Infrastructure.Identity.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Platinum.Infrastructure.Auth
{
    public interface IJwtFactory
    {
        Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
        RefreshToken GenerateRefreshToken(string ipAddress);
    }

    public class JwtFactory : IJwtFactory
    {
        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> userManager;

        private readonly JWTSettings jWTSettings;

        public JwtFactory(UserManager<ApplicationUser> userManager,
            IOptions<JWTSettings> jwtSettings)
        {
            this.userManager = userManager;
            jWTSettings = jwtSettings.Value;
            ThrowIfInvalidOptions(jWTSettings);
        }

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jWTSettings.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            //var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            //var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jWTSettings.ValidIssuer,
                audience: jWTSettings.ValidAudience,
                claims: claims,
                notBefore: jWTSettings.NotBefore,
                expires: jWTSettings.Expiration,
                signingCredentials: jWTSettings.SigningCredentials);

            return jwtSecurityToken;
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);


        private string RandomTokenString()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[40];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                // convert random bytes to hex string
                return BitConverter.ToString(randomBytes).Replace("-", "");
            }

        }
        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }


        private static void ThrowIfInvalidOptions(JWTSettings jwtSettings)
        {
            if (jwtSettings == null) throw new ArgumentNullException(nameof(jwtSettings));

            if (jwtSettings.DurationInMinutes <= 0)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(jwtSettings.Expiration));
            }

            if (jwtSettings.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JWTSettings.SigningCredentials));
            }

            if (jwtSettings.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JWTSettings.JtiGenerator));
            }
        }
    }
}
