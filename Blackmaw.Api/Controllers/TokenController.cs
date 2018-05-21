using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blackmaw.Dal.DbContext;
using Blackmaw.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Blackmaw.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly SignInManager<BlacmawUser> _signInManager;
        private readonly UserManager<BlacmawUser> _userManager;
        private readonly RoleManager<BlackmawRole> _roleManager;
        private readonly ILogger<TokenController> _logger;
        private readonly IConfiguration _configuration;

        public TokenController(
            UserManager<BlacmawUser> userManager,
            SignInManager<BlacmawUser> signInManager,
            RoleManager<BlackmawRole> roleManager,
            IConfiguration configuration,
            ILogger<TokenController> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._logger = logger;
            this._configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]LoginModel model)
        {
            var result = await this._signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            var user = this._userManager.Users.SingleOrDefault(r => r.UserName == model.Username);
            var q = GenerateJwtToken(model.Username, user);
            return Json(q);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new BlacmawUser
            {
                UserName = model.Username,
                Email = model.Username
            };
            var result = await this._userManager.CreateAsync(user, model.Password);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    this._logger.LogError($"Error updating user. {error.Code} : {error.Description}");
                }

                return BadRequest();
            }
            
            return await Authenticate(new LoginModel { Username = model.Username, Password = model.Password });
        }

        private object GenerateJwtToken(string username, BlacmawUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // This line makes UserName available off of the Identity object.
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(30));

            var token = new JwtSecurityToken(this._configuration["Issuer"], this._configuration["Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class RegisterModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            //public string Name { get; set; }
            //public string Email { get; set; }
            //public DateTime Birthdate { get; set; }
        }
    }
}