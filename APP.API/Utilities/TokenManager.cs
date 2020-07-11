using APP.Core.Model.ModelExt;
using APP.Repository.EFRepo.EntitiesExt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace APP.API.Utilities
{
    public static class TokenManager
    {
        /// <summary>
        /// Generate token for logged in user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Config"></param>
        /// <returns></returns>
        public static UsersExtModel_Auth GenerateToken(this UsersExtModel_Auth user, IConfiguration Config)
        {
            if (Config != null && user != null)
            {
                // Create Claims
                var UserClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, String.IsNullOrEmpty(user.RoleID)? "": user.RoleID),
                    // new Claim("another", user.Email)
                };

                var secretBytes = Encoding.UTF8.GetBytes(Config["Jwt:Key"]);
                var key = new SymmetricSecurityKey(secretBytes);
                var algorith = SecurityAlgorithms.HmacSha256;
                var signingcredentials = new SigningCredentials(key, algorith);
                var token = new JwtSecurityToken(
                    issuer: Config["Jwt:Issuer"],
                    audience: Config["Jwt:Issuer"],
                    claims: UserClaims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signingcredentials);

                user.Token = new JwtSecurityTokenHandler().WriteToken(token);
            }

            return user;
        }

        /// <summary>
        /// Deserialize token
        /// </summary>
        /// <param name="userClaims"></param>
        /// <returns></returns>
        public static IUser GetTokenInfo(ClaimsIdentity userClaims)
        {
            IUser user = null;
            if (userClaims != null)
            {
                user = new IUser();
                IList<Claim> claim = userClaims.Claims.ToList();
                user.Id = claim[0].Value;
                user.UserName = claim[1].Value;
            }

            return user;
        }
    }
}
