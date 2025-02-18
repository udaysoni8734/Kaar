using Microsoft.AspNetCore.Mvc;

namespace StockX.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test API is working!");
        }
    }
}