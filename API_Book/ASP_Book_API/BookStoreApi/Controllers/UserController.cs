using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Model;
using BookStoreApi.Service;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

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
                    string salt = account.salt;
                    string passSalt = user.password + salt;
                    string passHash = hashPass(passSalt);
                    if (passHash != account.password) {
                        return NotFound(new { result = "Don't have this account" });
                    }
                    var token = CreateToken(account);
                    await _service.EditData("UPDATE [USER] SET TOKEN = @TOKEN WHERE USERNAME = @USERNAME",new { token = token, username = user.username });
                    return Ok(new { result = "Success", token = token });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] InfoRegister user)
        {
            try
            {
                if (user.username == null || user.password == null) { return BadRequest("Error"); }
                else
                {
                    User account = await _service.GetAsync<User>("SELECT * FROM [USER] WHERE USERNAME = @USERNAME", new { username = user.username });
                    if (account != null)
                    {
                        return Ok("Account exists");
                    }
                    string salt = CreateString(5);
                    string passSalt = user.password + salt;
                    string passHash = hashPass(passSalt);
                    int number = await _service.GetAsync<int>("SELECT COUNT(*) FROM [USER]", new { });
                    number += 1;
                    string id = $"U{number}";
                    int n = await _service.EditData("INSERT INTO [USER] VALUES(@USERNAME,@PASSWORD,@FULLNAME,NULL,@ID,@SALT)", new { username = user.username, password = passHash, fullname = user.fullname, id = id, salt = salt });
                    if (n == 1) return Ok("Done");
                    else return BadRequest("Error while create account");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private string CreateString(int stringLength)
        {
            Random rd = new Random();

            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789!@$?_-";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        private string hashPass(string pass)
        {
            using (SHA1 sha1Hash = SHA1.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(pass);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
                //Console.WriteLine("The SHA1 hash of " + pass + " is: " + hash);
            }
        }
        private string CreateToken(User usr)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("username",usr.username),
                new Claim("fullname",usr.fullname),
                new Claim("id",usr.id)
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
