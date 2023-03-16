using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIProject.Data;
using WebAPIProject.Dtos;
using WebAPIProject.Models;

namespace WebAPIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }
    
        [HttpPost]
        public IActionResult Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            var userExist = _authRepository.UserExist(userForRegisterDto.UserName);
            if(userExist == false)
            {
                ModelState.AddModelError("UserName", "Username already exist");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createToUser = new User
            {
                UserName = userForRegisterDto.UserName
            };

            var createdUser = _authRepository.Register(createToUser, userForRegisterDto.Password);
            return StatusCode(201, createdUser);
        }

        public IActionResult Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authRepository.Login(userForLoginDto.UserName,userForLoginDto.Password);
            
            if(userToLogin == null)
            {
                return Unauthorized();
            }


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim (ClaimTypes.NameIdentifier, userToLogin.Id.ToString()),
                    new Claim (ClaimTypes.Name,userToLogin.UserName)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);

            
        }
    }
}
