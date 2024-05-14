
using Azure.Core;
using EFCore.Arvato.Dtos;
using EFCore.Arvato.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace EFCore.Arvato.Controllers
{
    [Route("api/[controller]/[action]")]   
    [ApiController]
    public class AuthController : ControllerBase
    {
       
        
        private readonly UserManager<ViewUser> _userManager;
        private readonly SignInManager<ViewUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<ViewUser> userManager
            , SignInManager<ViewUser> signInManager,
            IConfiguration configuration)
        {
           
            _userManager=userManager;
            _signInManager=signInManager;
            _configuration = configuration;
        }


        #region GenerateToken
        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody] LoginDto request)
        {
            string SecretKey = "asdlnsarjasuxvcknadnadspasasdaavcbnmsdjasdnlulusdfdsf";
            TimeSpan TokenLifeTime = TimeSpan.FromHours(8);
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name,request.UsernameOrEmail),                
                new(JwtRegisteredClaimNames.Email,request.UsernameOrEmail),              
            
            };

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience : "https://www.linkedin.com/in/baklhanztrk",
                claims : claims,
                expires : DateTime.UtcNow.AddDays(3),
                notBefore: DateTime.UtcNow,
                signingCredentials: creds
            );            
            var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(jwt);
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> LoginAccount(LoginDto request)
        {
            ViewUser? tUser =
                await _userManager.Users.FirstOrDefaultAsync(p => p.Email == request.UsernameOrEmail || p.UserName == request.UsernameOrEmail);

            if (tUser is null)  return BadRequest(new { Message = "Kullanıcı Bulunamadı" });


            var result = await _signInManager.CheckPasswordSignInAsync(tUser, request.Password, true);          


            if (result.IsLockedOut)
            {
                TimeSpan? timeSpan = tUser.LockoutEnd - DateTime.UtcNow;
                if (timeSpan is not null) return StatusCode(500, $"Şifrenizi 5 kere yanlış girdiğiniz için kullanıcınız {timeSpan.Value.TotalSeconds} saniye girişi yasaklanmıştır.");
                
            }

            if (result.IsNotAllowed) return StatusCode(500, $"Kullanıcınız henüz onaylanmamıştır, Lütfen daha sonra tekrar deneyiniz.");
            if (!result.Succeeded) return StatusCode(500, $"Şifreniz veya kullanıcı bilgileriniz yanlış.");

            string token = GenerateToken(tUser);

            return Ok(new { Token = token });
        }

        private string GenerateToken(ViewUser request)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,request.Id.ToString()),
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Email,request.Email)

            };

            var token = new JwtSecurityToken
                (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims:userClaims,
                expires:DateTime.UtcNow.AddDays(1),
                signingCredentials:credentials
                                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
