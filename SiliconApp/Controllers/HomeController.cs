using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using SiliconApp.Models;
using System.Security.Claims;
using SQLitePCL;
using SiliconApp.Data;
using Newtonsoft.Json;
using SiliconApp.Attributes;

namespace SiliconApp.Controllers
{
    [BreadcrumbActionFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }

        public async Task<JsonResult> GetUserInfo()
        {
            string userid = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
           ApplicationUser? applicationUser= await _context.ApplicationUsers.FindAsync(userid);
            if (applicationUser != null)
            {
                return Json(new {userImg=applicationUser.PersonImg });
            }
            return Json(null);
        }
    }
}
