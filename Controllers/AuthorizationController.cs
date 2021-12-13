using Microsoft.AspNetCore.Mvc;
using PGManagerApi.Authentication;

namespace PGManagerApi.Controllers
{
    [ApiController]
    [Route("auth")]
    [BasicAuthorization]
    public class AuthorizationController : ControllerBase
    {
        [HttpGet]
        public bool Authenticate()
        {
            return true;
        }
    }
}