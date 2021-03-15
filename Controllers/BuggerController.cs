using System;
using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class BuggerController : BaseController
    {
        private DataContext _context;

        public BuggerController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuthenticationError()
        {
            return "errorer";
        }

        [AllowAnonymous]
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1);

            if(thing == null)
            {
                return NotFound();
            }
            return Ok(thing);
        }
        [AllowAnonymous]
        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingstr = thing.ToString();
            return thingstr.ToString();
        }
        [AllowAnonymous]
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
