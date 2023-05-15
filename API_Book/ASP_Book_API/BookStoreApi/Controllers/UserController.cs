using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Model;
using BookStoreApi.Service;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IService _service;
        public UserController(IService dbService)
        {
            _service = dbService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] InfoLogin user)
        {
            try
            {
                if (user.username == null || user.password == null) { return BadRequest("Error"); }
                else
                {
                    User account = await _service.GetAsync<User>("SELECT * FROM [USER] WHERE USERNAME = @USERNAME", new { username = user.username });
                    if (account == null) { 
                        return NotFound(new { result = "Don't have this account"}); 
                    }
                    if (user.password != account.password) {
                        return NotFound(new { result = "Don't have this account" });
                    }
                    var token = CreateToken(account);
                    return Ok(new { result = "Success", token = token });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string CreateToken(User usr)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("username",usr.username),
                new Claim("fullname",usr.fullname)
            };
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            Debug.WriteLine(configuration.GetValue<string>("JWT:Secret"));
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Secret")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
