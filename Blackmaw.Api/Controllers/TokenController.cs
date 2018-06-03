﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Blackmaw.Dal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Blackmaw.Api.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly SignInManager<BlackmawUser> _signInManager;
        private readonly UserManager<BlackmawUser> _userManager;
        private readonly ILogger<TokenController> _logger;
        private readonly string _key = Environment.GetEnvironmentVariable("JwtKey");
        private readonly string _issuer = Environment.GetEnvironmentVariable("JwtIssuer");
        private readonly string _audience = Environment.GetEnvironmentVariable("JwtAudience");

        public TokenController(
            UserManager<BlackmawUser> userManager,
            SignInManager<BlackmawUser> signInManager,
            ILogger<TokenController> logger)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._logger = logger;
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("Auth")]
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

            var user = new BlackmawUser
            {
                UserName = model.Username,
                Email = model.Username
            };
            var result = await this._userManager.CreateAsync(user, model.Password);

            if (!result.Errors.Any())
            {
                return await Authenticate(new LoginModel {Username = model.Username, Password = model.Password});
            }

            foreach (var error in result.Errors)
            {
                this._logger.LogError($"Error updating user. {error.Code} : {error.Description}");
            }

            return BadRequest();

        }

        private object GenerateJwtToken(string username, BlackmawUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // This line makes UserName available off of the Identity object.
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(30));

            var token = new JwtSecurityToken(this._issuer, _audience,
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