using Budget.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
// Test controller
namespace Budget.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/v1")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public string GetUsersAsync([FromHeader(Name = "Authorization")] string x)
        {

            return x.Remove(0, 7);
        }
    }
}
