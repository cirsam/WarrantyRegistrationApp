using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;
        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IRepository<Login> repository, IConfiguration config, ILogger<LoginController> logger)
        {
            _config = config;
            _repository = repository;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = Authenticate(login);
            var passwordHash = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

            if (user == null)
            {
                _logger.LogWarning("User does not exist.");

                return NotFound("User not found");
            }
            
            if (passwordHash!=PasswordVerificationResult.Success)
            {
                return BadRequest("bad UserName Or PasswordMessage");
            }

            var token = await Generate(user);
            _logger.LogInformation("User is logged in.");

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

            _logger.LogInformation("JWT token created.");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private IdentityUser Authenticate(Login login)
        {
            var currentUser = _userManager.Users.FirstOrDefault(o => o.UserName.ToLower() == login.Username.ToLower());
            if (currentUser != null)
            {
                _logger.LogInformation("User is autenticated.");

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
                    var callbackUrl = $"https://{Request.Host}/api/login/ConfirmEmail?userId={user.Id}";

                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage(
                             new System.Net.Mail.MailAddress("sender@mydomain.com", "Web Registration"),
                             new System.Net.Mail.MailAddress(user.Email));
                                 m.Subject = "Email confirmation";
                                 m.Body = string.Format("Dear {0}, <BR/>Thank you for your registration, please click on the below link to complete your registration: <a href=\"{1}\" title=\"User Email Confirm\">{1}</a>",
                                 user.UserName,callbackUrl);
                                 m.IsBodyHtml = true;
                         System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mydomain.com");
                         smtp.Credentials = new System.Net.NetworkCredential("sender@mydomain.com", "password");

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
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(token);
                if (user == null)
                {
                    return NotFound($"Unable to load user with id '{token}'.");
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");

            if (returnUrl != null)
            {
                return Ok(returnUrl);
            }
            else
            {
                return RedirectToAction(); 
            }
        }

    }
}
