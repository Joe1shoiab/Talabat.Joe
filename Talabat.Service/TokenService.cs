using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<string> CreateTokenAsync(AppUser appUser, UserManager<AppUser> userManager)
        {
            var AuthClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.DisplayName),
                
            };

            var userRoles = userManager.GetRolesAsync(appUser);
            foreach (var userRole in userRoles.Result)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

            var token = new JwtSecurityToken(
                    issuer: configuration["JWT:Issuer"],
                    audience: configuration["JWT:Audience"],
                    claims: AuthClaims,
                    expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
                    signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256)
                    );

            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
