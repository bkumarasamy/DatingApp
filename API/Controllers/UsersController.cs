using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _mapper = mapper;
            _userRepository = userRepository;

        }
        // public UsersController(DataContext context)
        // {
        //     _context = context;
        // }
        // [Authorize(Roles ="Admin")]
        [HttpGet]
        // [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> Getuser([FromQuery]UserParams userParams)
        {
            // return await _context.User.ToListAsync();
            var user = await _userRepository.GetUserByNameAsync(User.GetUsername());
            userParams.CurrentUsername = (string.IsNullOrEmpty(User.GetUsername())?user.UserName:User.GetUsername());

            if(string.IsNullOrEmpty(userParams.Gender))
            {
            userParams.Gender = user.Gender == "male"? "female": "male";
            }

            var users = await _userRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,
                users.TotalCount,users.TotalPages);

            return Ok(users);

            // var users=await _userRepository.GetUserAsync();
            // var usersToReturn=_mapper.Map<IEnumerable<MemberDTO>>(users);
            // return Ok(usersToReturn);

            //return Ok(await _userRepository.GetUserAsync());
        }
        //api/Getuser/lisa
        // [Authorize(Roles ="Member")]
        [HttpGet("{username}",Name ="GetUser")]
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
            // var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // var username=User.GetuserName();
            var user = await _userRepository.GetUserByNameAsync(User.GetUsername());

            _mapper.Map(memberUpdateDto, user);

            if(!string.IsNullOrEmpty(user.UserName)) _userRepository.update(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("failed to update the user");

        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByNameAsync(User.GetUsername());
            
            var result = await _photoService.AddPhotoAsync(file);

            if(result.Error!=null) return BadRequest(result.Error.Message);

            var photo=new Photo
            {
                URL=result.SecureUrl.AbsoluteUri,
                PublicID=result.PublicId
            };
            if(user.Photos.Count==0)
            {
                photo.IsMain=true;
            }

            user.Photos.Add(photo);

            if(await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDTO>(photo);  
                return CreatedAtRoute("GetUser",new{username=user.UserName},_mapper.Map<PhotoDTO>(photo));         
            }
                

            return BadRequest("problem adding photo");

        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user=await _userRepository.GetUserByNameAsync(User.GetUsername());

            var photo=user.Photos.FirstOrDefault(x => x.ID == photoId);

            if(photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain=user.Photos.FirstOrDefault(x => x.IsMain);
            if(currentMain!=null) currentMain.IsMain=false;
            photo.IsMain=true;

            if(await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user=await _userRepository.GetUserByNameAsync(User.GetUsername());

            var photo=user.Photos.FirstOrDefault(x=>x.ID==photoId);
                
            if(photo==null) return NotFound();

            if(photo.IsMain) return BadRequest("You cannot Delete your Main photo");

            if(photo.PublicID!=null)
            {
                var result=await _photoService.DeletePhotoAsync(photo.PublicID);
                if(result.Error!=null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to delete the photo");
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