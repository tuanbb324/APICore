using ACBSChatbotConnector.Helpers;
using ACBSChatbotConnector.Models;
using ACBSChatbotConnector.Repositories;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UserService.Models;

namespace ACBSChatbotConnector.Services.Implement
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IDapperDA _dapper;


        public TokenService(IConfiguration configuration, IDapperDA dapper)
        {
            _configuration = configuration;
            _dapper = dapper;

        }

        public string GenerateJwtToken(CMS_User user)
        {
            
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var permClaims = new List<Claim>();

                permClaims.Add(new Claim("Id", user.Id.ToEmptyString()));
                permClaims.Add(new Claim("Username", user.Username.ToEmptyString()));

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                  _configuration["Jwt:Issuer"],
                  permClaims,
                  expires: DateTime.Now.AddDays(_configuration["Jwt:Expires"].ToInt()),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public async Task NewRefreshToken(int userId, string refreshToken)
        {
            try
            {
                DynamicParameters _dbParams = new DynamicParameters();
                var refreshTokenExpiryTime = DateTime.Now.AddDays(_configuration["Jwt:RefreshTokenValidityInDays"].ToInt());

                _dbParams.Add("UserId", userId);
                _dbParams.Add("RefreshToken", refreshToken);
                _dbParams.Add("RefreshTokenExpiryTime", refreshTokenExpiryTime);
                var res = await _dapper.InsertAsync<CMS_User_RefreshTokens>("[dbo].[CMS_User_REFRESH_TOKEN_NEW]", _dbParams, CommandType.StoredProcedure);

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        public CMS_User GetUserFromClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
        {

            IEnumerable<Claim> claims = claimsPrincipal.Claims;
            var Id = claims.Where(p => p.Type == "Id").FirstOrDefault()?.Value;
            var Username = claims.Where(p => p.Type == "Username").FirstOrDefault()?.Value;

            CMS_User user = new CMS_User();
            user.Id = Id.ToInt();
            user.Username = Username;

            return user;

        }
        public async Task<RefreshTokenModel> GetRefreshToken(int userId)
        {
            try
            {
                DynamicParameters dbParams = new DynamicParameters();
                dbParams.Add("UserId", userId);
                var result = await _dapper.GetAsync<RefreshTokenModel>("[dbo].[CMS_User_REFRESH_TOKEN_GET]", dbParams, CommandType.StoredProcedure);
                var res =  result.FirstOrDefault();
                if (res.RefreshToken == null)
                {
                    return null;
                }
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task UpdateRefreshToken(int userId, string refreshToken)
        {
            try
            {
                DynamicParameters dbParams = new DynamicParameters();

                dbParams.Add("UserId", userId);
                dbParams.Add("RefreshToken", refreshToken);

                await _dapper.ExecuteAsync("[dbo].[CMS_User_REFRESH_TOKEN_UPDATE]", dbParams, CommandType.StoredProcedure);


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
