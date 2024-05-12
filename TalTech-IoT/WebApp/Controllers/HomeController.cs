using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

/// <inheritdoc />
public class HomeController : Controller
{
    /// <inheritdoc />
    public HomeController() {}

    /// <inheritdoc>
    ///     <cref></cref>
    /// </inheritdoc>
    public IActionResult Index()
    {
        return View();
    }
}