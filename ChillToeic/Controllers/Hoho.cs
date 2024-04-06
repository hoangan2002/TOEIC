using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class Hoho : Controller
    {
        [Authorize]   
        public IActionResult Index()
        {
            return Ok("ok");
        }
    }
}
