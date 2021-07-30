using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
       // public DataContext _context { get; }
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly ITokenService _TokenService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
                                 ITokenService TokenService, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _TokenService = TokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExist(registerDto.Username))
            {
                return BadRequest("UserName is taken");
            }
            var user = _mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.Username.ToLower();

            var result=await _userManager.CreateAsync(user,registerDto.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult= await _userManager.AddToRoleAsync(user,"Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                Username = user.UserName,
                Token = await _TokenService.CreateToken(user),
                KnownUs = user.KnownUs,
                Gender = user.Gender
            };

        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            var result=await _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);

            if(!result.Succeeded) return Unauthorized();

            return new UserDto
            {
                Username = loginDto.Username,
                Token = await _TokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.URL,
                KnownUs = user.KnownUs,
                Gender = user.Gender
            };

        }

        private async Task<bool> UserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}