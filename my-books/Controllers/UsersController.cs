using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using my_books.Data.Models;
using my_books.Data.Services.ViewModel;
using Microsoft.Extensions.Configuration;
using AuthenticationPlugin;
using my_books.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
       
        public AppDbContext _context;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public UsersController(AppDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _auth = new AuthService(_configuration);
            _context = context;
        }

        [HttpPost("register-user")]

        public IActionResult RegisterUser([FromBody]UserVM user)
        {
            var _user = _context.Users.Where(u => u.Email == user.Email).SingleOrDefault();

            if (_user != null)
            {
                return BadRequest("User with same email exists");
            }
            else
            {
                var userObj = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    Password = SecurePasswordHasherHelper.Hash(user.Password),
                    Role = "Users"
                };
                _context.Users.Add(userObj);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }


        }

        [HttpPost("login-user")]
        public IActionResult Login([FromBody]UserVM user)
        {
            var userEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);

            if (userEmail == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
            {
                return Unauthorized();
            }

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
               new Claim(ClaimTypes.Email, user.Email),
               new Claim(ClaimTypes.Role, userEmail.Role)
            };
            var token = _auth.GenerateAccessToken(claims);

            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id
            });
        }

    }
}
