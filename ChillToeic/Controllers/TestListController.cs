using ChillToeic.Models.Entity;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var tests = _testService.GetTestIsTrue();
            return View(tests);
        }
		public IActionResult Detail(int testId)
		{
            var testDetail = _testService.GetTestById2(testId).FirstOrDefault();

            if (testDetail == null)
            {
                return NotFound();
            }

            return View(testDetail);
        }
		
    }
}