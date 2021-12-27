using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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

//https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=visual-studio

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
            var passwordHash = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (user == null)
            {
                return NotFound("User not found");
            }
            
            if (passwordHash!=PasswordVerificationResult.Success)
            {
                return BadRequest("bad UserName Or PasswordMessage");
            }

            var token = await Generate(user);
            return Ok(token);
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            try
            {
                var user = new IdentityUser { UserName = register.Username, Email = register.Email };
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, register.Password);

                var result = await _userManager.CreateAsync(user);
                
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customers");//All users are first added as customers
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = $"https://{Request.Host}/api/login/ConfirmEmail?userId={user.Id}&code={code}";

                    return Ok(callbackUrl);
                }

                return BadRequest("User already exist");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId,string code)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"Unable to load user with id '{userId}'.");
                }

                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);

                return Ok("Account comfirmed");
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
