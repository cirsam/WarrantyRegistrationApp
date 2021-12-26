using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WarrantyRegistrationApp.Models;
using WarrantyRegistrationApp.Repository;

namespace WarrantyRegistrationApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IRepository<Login> _repository;
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IRepository<Login> repository, IConfiguration config)
        {
            _config = config;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = Authenticate(login);

            if (user != null)
            {
                var token = await Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private async Task<string> Generate(IdentityUser user)
        {
            var now = DateTime.Now;
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["WarrantyReg_JWT:Key"]));
            var credentialsToken = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new Claim[]
           {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault())
           };

            var issuer = _config["WarrantyReg_JWT:IssuerUrl"];
            var audience = _config["WarrantyReg_JWT:AudienceUrl"];
            var expires = _config["WarrantyReg_JWT:Expires"];

            var token = new JwtSecurityToken(
                            issuer: issuer,
                            audience: audience,
                            claims: claims,
                            notBefore: now,
                            expires: now.AddMinutes(int.Parse(expires)),
                            signingCredentials: credentialsToken);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IdentityUser Authenticate(Login login)
        {
            var currentUser = _userManager.Users.FirstOrDefault(o => o.UserName.ToLower() == login.Username.ToLower());
            if (currentUser != null)
            {
                return currentUser;
            }

            return null;
        }

    }
}
