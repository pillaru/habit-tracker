using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Controllers;

public class HomeController : Controller
{
    public ViewResult Index()
    {
        return View();
    }
}
