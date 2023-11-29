using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RAS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnauthoriseController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "You are Authorised without token";
        }
    }
}
