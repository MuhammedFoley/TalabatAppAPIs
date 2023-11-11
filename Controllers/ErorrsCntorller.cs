using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatAppAPIs.Erorrs;

namespace TalabatAppAPIs.Controllers
{
    [Route("erorr/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErorrsCntorller : ControllerBase
    {
        public ActionResult Erorr(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
