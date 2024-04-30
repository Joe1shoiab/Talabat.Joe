using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.Core.Entities;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)] // to hide this controller from swagger
    public class ErrorsController : ControllerBase
    {
        
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
