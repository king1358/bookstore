using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Models;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Collections;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]



    public class AccountLogin
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class LoginResult
    {
        public string Result { get; set; }
        public string Token { get; set; }
    }
    public class AuthController : ControllerBase
    {

        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public LoginResult Login([FromForm] AccountLogin user)
        {
            using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
            {
                conn.Open();
                string sqlquery = "SELECT * FROM [user] WHERE USERNAME = @username";
                List<User> usr = conn.Query<User>(sqlquery, new { username = user.username }).ToList();
                //return OK(usr[0].dat);


                if (usr.Count() != 0)
                {
                    //var someString = usr[0].createdate;
                    string salt = usr[0].salt;
                    //byte[] bytes = Encoding.ASCII.GetBytes(salt);
                    string passSalt = salt + user.password;
                    string passHash = hashPass(passSalt);
                    if (passHash == usr[0].password)
                    {
                        return new LoginResult{ Result = "Success",Token = CreateToken(usr[0])};
                    }
                    else return new LoginResult { Result = "Fail", Token = null };
                }
                else
                {
                    return new LoginResult { Result = "No user", Token = null };
                }

            }
        }


        //[HttpPost("register")]
        //public LoginResult Register([FromForm] AccountLogin user)
        //{
        //    using (var conn = new SqlConnection("Data Source=LAPTOP-4N02CR39;Initial Catalog=BOOKSTORE;User ID=sa1;Password=svcntt;TrustServerCertificate=True"))
        //    {
        //        conn.Open();
        //        string sqlquery = "SELECT * FROM [user] WHERE USERNAME = @username";
        //    }
        //}

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
                new Claim("username",usr.userName),
                new Claim("role", usr.role)
            };
            Debug.WriteLine(_config.GetValue<string>("JWT:Secret"));
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:Secret")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

    }
}
