using LinkDev.Talabat.Apis.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Buggy
{
    public class BuggyController : BaseApiController
    {
        [HttpGet("notfound")]  //get : api/buggy/notfound

        public IActionResult GetNotFoundRequest()
        {
            return NotFound(new { StatusCode = 404, Message = "notfound" }); // 404
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            throw new Exception(); //500
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new { StatusCode = 400, Message = "Bad Request" }); // 400
        }

        [HttpGet("badrequest/{id}")]   // validation error
        public IActionResult GetValidationError(int id)
        {
            return Ok(); 
        }


        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorizedError()
        {
            return Unauthorized(); // 401
        }

        [HttpGet("forbidden")]
        public IActionResult GetForbiddenRequest()
        {
            return Forbid(); // 401
        }
    }

}
