using Microsoft.AspNetCore.Mvc;

namespace IndeedClone.Modules.IndeedClone.Controllers
{
    public class FooterInfoController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> About()
        {
            return View("about");
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            return View("contacts");
        }

        [HttpGet]
        public async Task<IActionResult> FAQ()
        {
            return View("faq");
        }

        [HttpGet]
        public async Task<IActionResult> Privacyy()
        {
            return View("privacy");
        }

        [HttpGet]
        public async Task<IActionResult> Terms()
        {
            return View("terms");
        }

        [HttpGet]
        public async Task<IActionResult> Pricing()
        {
            return View("pricing");
        }

        [HttpGet]
        public async Task<IActionResult> RecruitmentSolutions()
        {
            return View("recruitmentsolutions");
        }

        [HttpGet]
        public async Task<IActionResult> CareerAdvice()
        {
            return View("careeradvice");
        }

        [HttpGet]
        public async Task<IActionResult> CookiePolicy()
        {
            return View("cookiepolicy");
        }

        [HttpGet]
        public async Task<IActionResult> Accessibility()
        {
            return View("accessibility");
        }
    }
}
