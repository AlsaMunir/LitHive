using Microsoft.AspNetCore.Mvc;
using Domain.Models;

using System.Threading.Tasks;

public class LoginController : Controller
{
    private readonly IUserSevice _userService;

    public LoginController(IUserSevice userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult LoginForm()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginForm(string email, string password)
    {
        var user = await _userService.ValidateUserAsync(email, password);

        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Email", user.Email);
            return RedirectToAction("Fiction", "Home");
        }
        
        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Fiction", "Home");
    }
}