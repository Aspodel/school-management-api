using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolManagement.Api.Models;
using SchoolManagement.Api.Settings;
using SchoolManagement.Core.Entities;
using SchoolManagement.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IOptionsMonitor<JwTokenConfig> _tokenConfig;

        public AuthController(UserManager userManager, SignInManager<User> signInManager, IOptionsMonitor<JwTokenConfig> tokenConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenConfig = tokenConfig;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!passwordCheck.Succeeded)
                return BadRequest(new { message = "Username or password is incorrect" });

            //var tokenConfig = _jwTokenConfig

            return Ok();
        }

        private async Task<string> GenerateToken(User user, JwTokenConfig tokenConfig)
        {
            var handler = new JwtSecurityTokenHandler();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.UserName, "TokenAuth"),
                new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) }
                    .Union(roles.Select(role => new Claim(ClaimTypes.Role, role)))
                );

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.JWT_Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //var descriptor = new SecurityTokenDescriptor
            //{

            //}


            var securityToken = handler.CreateToken(new SecurityTokenDescriptor());
            var token = handler.WriteToken(securityToken);

            return token;
        }
    }
}
