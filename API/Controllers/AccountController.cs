using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext _context { get; }
/*
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(string username,string password)
        {
           // var user=await(_context.User.SingleOrDefaultAsync(m=>m.Username==Username));
            if(await(UserExist(username)))
            {
                return BadRequest("Invalid User");
            }

            using var hmac = new HMACSHA512();

            var user=new AppUser
            {
                Username=username.ToLower(),
                PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt=hmac.Key
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDto loginDto)
        {
            var user = await _context.User
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }

            return user;

        }
*/


        private readonly ITokenService _TokenService;
        public AccountController(DataContext context, ITokenService TokenService)
        {
            _TokenService = TokenService;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username))
            {
                return BadRequest("UserName is taken");
            }
            using var Hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username = registerDto.Username.ToLower(),
                PasswordHash = Hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = Hmac.Key
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            
            return new UserDto
            {
                Username=user.Username,
                Token=_TokenService.CreateToken(user)
            };
            
            // return user;

        }

      
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.User
                .SingleOrDefaultAsync(x => x.Username == loginDto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid Password");
            }

            return new UserDto
            {
                Username=loginDto.Username,
                Token=_TokenService.CreateToken(user)
                
            };

        }

        private async Task<bool> UserExist(string username)
        {
            return await _context.User.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}