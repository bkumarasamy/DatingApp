using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        // public DataContext _context { get; }
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }
        // public UsersController(DataContext context)
        // {
        //     _context = context;
        // }
        [HttpGet]
        // [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> Getuser()
        {
            // return await _context.User.ToListAsync();

            var users=await _userRepository.GetMembersAsync();
            
            return Ok(users);

            // var users=await _userRepository.GetUserAsync();
            // var usersToReturn=_mapper.Map<IEnumerable<MemberDTO>>(users);
            // return Ok(usersToReturn);

            //return Ok(await _userRepository.GetUserAsync());
        }
        //api/Getuser/lisa
        [HttpGet("{username}")]
        //[Authorize]
        public async Task<ActionResult<MemberDTO>> GetuserName(string username)
        {
            return await _userRepository.GetMemberAsync(username);
            //var users=await _userRepository.GetUserByNameAsync(username);
            //return _mapper.Map<MemberDTO>(users);

            // return Ok(await _userRepository.GetUserByNameAsync(username));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user=await _userRepository.GetUserByNameAsync(username);

            _mapper.Map(memberUpdateDto,user);

            _userRepository.update(user);

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("failed to update the user");
            
        }




        // //api/Getuser/2
        // [HttpGet("{id}")]
        // [Authorize]
        // public async Task<ActionResult<AppUser>> Getuser(int id)
        // {
        //     //return await _context.User.FindAsync(id);
        //     // var users=await _userRepository.GetUserByIdAsync(id));
        //     // return Ok(users);

        //     return Ok(await _userRepository.GetUserByIdAsync(id));
        // }


    }
}