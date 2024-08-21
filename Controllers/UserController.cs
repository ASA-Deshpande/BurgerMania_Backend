using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BurgerMania.Models;
using BurgerMania.Data;
using Microsoft.EntityFrameworkCore;
using BurgerMania.Mappers;
using BurgerMania.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BurgerMania.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Policy;

namespace BurgerMania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BurgerManiaContext _context;
        private readonly UserMapper _usermapper;
        private readonly JwtConfig _jwtConfig;

        public UserController(BurgerManiaContext context, IOptions<JwtConfig> jwtConfig)
        {
            _context = context;
            _usermapper = new UserMapper();
            _jwtConfig = jwtConfig.Value;
        }

        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetAll()
        {
            var users =  _context.Users
               .ToList();

            var userDTOs = users.Select(user => _usermapper.MapToDTO(user)).ToList();

            if(!userDTOs.Any())
            {
                return NotFound();
            }

            return Ok(userDTOs);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTO> GetById(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user == null) 
            {
                return NotFound();
            } 

            var userDTO = _usermapper.MapToDTO(user);
            return Ok(userDTO);

            //return Ok(user);
        }


        //[HttpPost("/phone")]
        //public ActionResult<User> FindByPhone([FromBody] string phoneNumber)
        //{
        //    var user = _context.Users.FirstOrDefault(u => u.phnNumber == phoneNumber);

        //    if (user == null)
        //    {
        //        return NotFound() ;
        //    }
        //    else
        //    {
        //        return Ok(user.ID);
        //    }
        //}


        [HttpPost("/register")]
        public ActionResult<UserDTO> CreateUser(UserDTO userdto)
        {
  
            var existingUser = _context.Users.FirstOrDefault(u => u.phnNumber == userdto.phnNumber);

            if (existingUser != null)
            {
                return Conflict($"User with phone number {userdto.phnNumber} already exists!");
            }

            var user = _usermapper.MapToEntity(userdto);
            user.ID = Guid.NewGuid();
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userdto.Password);

            _context.Users.Add(user);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.ID))
                {
                    return Conflict("User with this ID already exists!");
                }
                else
                {
                    throw;
                }

            }

            userdto.ID = user.ID;

            return CreatedAtAction(nameof(GetById), new { id = user.ID }, userdto);

        }

        [HttpPost("/login")]
        public ActionResult<UserDTO> Login(LoginDTO logindto)
        {
            var user = _context.Users.SingleOrDefault(u => u.phnNumber == logindto.phnNumber);
            if (user == null || !BCrypt.Net.BCrypt.Verify(logindto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials.");
            }

            //Console.WriteLine($"Login route: phnnum - {user.phnNumber}");

            var token = GenerateJwtToken(user);

            //var cookieOptions = new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = false,
            //    SameSite = SameSiteMode.None,
            //    Path = "/",
            //};

            //Response.Cookies.Append("access_token", token, cookieOptions);

            return Ok(new { Token = token, status = "Success", UserDTO = _usermapper.MapToDTO(user), message = "Logged in successfully" });
            //return Ok(new {Token = token, UserDTO = _usermapper.MapToDTO(user), message = "Logged in successfully" });
        }

        [NonAction]
        private string GenerateJwtToken(User user)
        {
            //Console.WriteLine($"Generate method: phnnum - {user.phnNumber}");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.phnNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.ExpiryInMinutes),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //[HttpPost]
        //public ActionResult<User> CreateUser(User user)
        //{

        //    var existingUser = _context.Users.FirstOrDefault(u => u.phnNumber == user.phnNumber);

        //    if (existingUser != null)
        //    {
        //        return Conflict($"User with phone number {user.phnNumber} already exists!");
        //    }


        //    user.ID = Guid.NewGuid();
        //    _context.Users.Add(user);

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch(DbUpdateException)
        //    {
        //        if(UserExists(user.ID))
        //        {
        //            return Conflict("User with this ID already exists!");
        //        }
        //        else
        //        {
        //            throw;
        //        }

        //    }

        //    return CreatedAtAction(nameof(GetById), new { id = user.ID }, user);

        //}

        [HttpPut("{id}")]
        public ActionResult<UserDTO> UpdateUser(Guid id, UserDTO userdto)
        {
            if (id != userdto.ID)
            {
                return BadRequest();
            }

            var user = _context.Users.FirstOrDefault(u => u.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            var userToMap = _usermapper.MapToEntity(userdto);
            
            if(!string.IsNullOrEmpty(userdto.phnNumber))
            {
                user.phnNumber = userToMap.phnNumber;
            }

            if (!string.IsNullOrEmpty(userToMap.Name))
            {
                user.Name = userToMap.Name;
            }
            

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(user.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return NoContent();

        }

        //[HttpPut("{id}")]
        //public ActionResult<User> UpdateUser(Guid id, User user)
        //{
        //    if(id != user.ID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(user.ID))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }

        //    }

        //    return NoContent();

        //}

        [HttpDelete]
        public IActionResult DeleteById(Guid id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }

            return NoContent();

        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(u => u.ID == id);
        }

    }
}
