using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class TestListController : Controller
    {
        private readonly TestService _testService;

        public TestListController(TestService testService)
        {
            _testService = testService;
        }

        public IActionResult TestList()
        {
            var tests = _testService.GetAllTests();

            return View(tests);
        }
    }
}
