using Microsoft.AspNetCore.Mvc;

namespace AttendanceMonitoringApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        [HttpGet("/TestConnection")]
        public async Task<ActionResult<String>> TestConnection()
        {
            return Ok("Connection has been established!");
        }
    }
}
