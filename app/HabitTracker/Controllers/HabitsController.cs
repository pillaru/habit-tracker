using Microsoft.AspNetCore.Mvc;
using HabitTracker.Models;

namespace HabitTracker.Controllers;

public class HabitsController : Controller
{
    [HttpGet]
    public ViewResult New()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ViewResult New(NewHabitDto newHabit)
    {
        return View(newHabit);
    }
}

