using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthoriseController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "You are Authorised User";
        }
    }
}
