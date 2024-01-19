using FinalProject.Back.Contexts;
using FinalProject.Data.Dtos;
using FinalProject.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly CertificationDbContext context;

        public AccountController(IConfiguration configuration, CertificationDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }


        [HttpPost("register")]
        public IActionResult Register(UserDto userDto)
        {
            var emailCount = context.Users.Count(u => u.Email == userDto.Email);
            if (emailCount > 0)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //encrypt password
            var passwordHasher = new PasswordHasher<User>();
            var encryptedPassword = passwordHasher.HashPassword(new User(), userDto.Password);

            User user = new User()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone ?? "",
                Address = userDto.Address,
                Password = encryptedPassword,
                Role = "candidate",
                CreatedAt = DateTime.Now
            };

            context.Users.Add(user);
            context.SaveChanges();

            var jwt = CreateJWToken(user);

            UserProfileDto userProfileDto = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            var response = new
            {
                Token = jwt,
                user = userProfileDto
            };

            return Ok(response);
        }

        //POST api/account/login
        [HttpPost("login")]
        public IActionResult Login(LoginDto model)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == model.Email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return BadRequest(ModelState);
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(new User(), user.Password, model.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Password", "Invalid password");
                return BadRequest(ModelState);
            }

            var jwt = CreateJWToken(user);

            UserProfileDto userProfileDto = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            var response = new
            {
                Token = jwt,
                user = userProfileDto
            };

            return Ok(response);

        }

        //Authorize Authenticated Users
        [Authorize(Roles = "admin")]
        [HttpGet("GetAuthenticatedUser")]
        public IActionResult GetAuthenticatedUser()
        {
            return Ok("You are authorized user (client or admin)");
        }

        //Authorize Admin Users
        [Authorize(Roles = "admin")]
        [HttpGet("GetAdminUser")]
        public IActionResult GetAdminUser()
        {
            return Ok("You are authorized user (admin)");
        }



        //GET api/account/GetAllUsers
        [Authorize(Roles = "admin")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = context.Users.ToList();
            return Ok(users);
        }

        //GET api/account/GetProfile
        [HttpGet("GetProfile")]
        public IActionResult GetProfile(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            UserProfileDto userProfileDto = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(userProfileDto);
        }

        //PUT api/account/ChangePassword
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, oldPassword);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("OldPassword", "Invalid password");
                return BadRequest(ModelState);
            }

            var encryptedPassword = passwordHasher.HashPassword(new User(), newPassword);
            user.Password = encryptedPassword;
            context.SaveChanges();

            return Ok();
        }

        //PUT api/account/UpdateProfile
        [HttpPut("UpdateProfile")]
        public IActionResult UpdateProfile(int id, UserDto userDto)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            var emailCount = context.Users.Count(u => u.Email == userDto.Email && u.Id != id);
            if (emailCount > 0)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return BadRequest(ModelState);
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone ?? "";
            user.Address = userDto.Address;

            context.SaveChanges();

            return Ok();
        }



        //DELETE api/account/DeleteAccount
        [HttpDelete("DeleteAccount")]
        public IActionResult DeleteAccount(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok();
        }

        //create json web token
        private string CreateJWToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", ""+user.Id),
                new Claim("role", user.Role)
            };
            string strKey = configuration["JwtSettings:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);


            return jwt;
        }
    }

}
