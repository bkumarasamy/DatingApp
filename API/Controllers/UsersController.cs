using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        public DataContext _context { get; }
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>>  Getuser()
        {
            return await _context.User.ToListAsync();
        }
        //api/Getuser/2
        [HttpGet("{id}")]
        [Authorize]
        public async  Task<ActionResult<AppUser>> Getuser(int id)
        {
            return await _context.User.FindAsync(id);

        }
    }
}