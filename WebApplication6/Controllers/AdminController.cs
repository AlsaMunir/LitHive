using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace WebApplication6.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    { 
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
