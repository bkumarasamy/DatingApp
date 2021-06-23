using API.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController : ControllerBase
    {
        // public DataContext _context { get; }
        // public BaseApiController(DataContext context)
        // {
        //     _context = context;
        // }

        
    }
}