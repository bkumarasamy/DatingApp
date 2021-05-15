using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        public DataContext _context { get; }
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>>  Getuser()
        {
            return await _context.User.ToListAsync();
        }
        //api/Getuser/2
        [HttpGet("{id}")]
        public async  Task<ActionResult<AppUser>> Getuser(int id)
        {
            return await _context.User.FindAsync(id);

        }
    }
}