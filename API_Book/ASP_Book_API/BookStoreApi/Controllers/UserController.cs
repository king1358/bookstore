using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStoreApi.Model;
using BookStoreApi.Interface;
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
        private readonly IUser _userRepository;
        private readonly ICart _cartRepository;
        public UserController(IUser userRepository, ICart cartRepository)
        {
            _userRepository = userRepository;
            _cartRepository = cartRepository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] InfoLogin user)
        {
            try
            {
                if (user.username == null || user.password == null) { return BadRequest("Username or password invalid"); }
                else
                {
                    User account = await _userRepository.FindUser(user.username);
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
                    bool res = await _userRepository.SetToken(user.username,token);
                    if (res == true) return Ok(new { result = "Success", token = token });
                    else return BadRequest("Error while create account");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] InfoRegister user)
        {
            try
            {
                if (user.username == null || user.password == null) { return StatusCode(406,new { message = "Username or password is null" }); }
                else
                {
                    User account = await _userRepository.FindUser(user.username);
                    if (account != null)
                    {
                        return StatusCode(406, new { result = "Account exists" });
                    }
                    string salt = CreateString(5);
                    string passSalt = user.password + salt;
                    string passHash = hashPass(passSalt);
                    int id = await _userRepository.CountUser();
                    id += 1;
                    DateTime created_time = DateTime.Now;
                    bool res = await _userRepository.CreateUser(user, id, salt, passHash, created_time);
                    if (res == true)
                    {
                        bool res2 = await _cartRepository.CreateCartForUser(id, id);
                        if (res2 == true) return Ok(new { result = "Successfull" });
                        else return BadRequest(new { reslt = "Error while create account" });
                    }
                    else return BadRequest(new { reslt = "Error while create account" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Province")]
        public async Task<IActionResult> GetProvince()
        {
            try
            {
                List<Province> provinces = await _userRepository.GetProvinceList();
                return Ok(provinces);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("District")]
        public async Task<IActionResult> GetDistrict(int id_province)
        {
            try
            {
                List<District> districts = await _userRepository.GetDistrictList(id_province);
                return Ok(districts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Ward")]
        public async Task<IActionResult> GetWard(string id_district)
        {
            try
            {
                List<Ward> wards = await _userRepository.GetWardList(id_district);
                return Ok(wards);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("AddressUser")]
        public async Task<IActionResult> GetAddressUser(int id_user, string token)
        {
            try
            {
                if (id_user == -1) return StatusCode(406, new { message = "Please login" });
                bool verify = await _userRepository.Verify(id_user,token);
                if (verify == false) return StatusCode(406, new { message = "You don't have premission to do this" });
                List<Address> addressUser = await _userRepository.GetAddressList(id_user);
                return Ok(addressUser);
            }
            catch(Exception ex)
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
                new Claim("id", usr.id.ToString())
            };
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            //Debug.WriteLine(configuration.GetValue<string>("JWT:Secret"));
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JWT:Secret")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
    }
}
