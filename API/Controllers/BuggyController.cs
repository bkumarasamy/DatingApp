using System;
using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;
        public BuggyController(DataContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-Found")]
        public ActionResult<string> GetNotFound()
        {
            var thing=_context.Users.Find(-1);
            
            if(thing==null) return NotFound();
            
            return Ok(thing);
        }

        [HttpGet("Server-Error")]
        public ActionResult<string> GetServerError()
        {
            // try
            // {
             var thing=_context.Users.Find(-1);

             var thingtoReturn=thing.ToString();

             return thingtoReturn;
            // }
            // catch (Exception ex)
            // {
            //  return StatusCode(500,ex.ToString());
            // }

        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }

    }
}