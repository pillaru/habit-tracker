using Microsoft.AspNetCore.Mvc;
using HabitTracker.Models;
using HabitTracker.Domain;

namespace HabitTracker.Controllers;

public class HabitsController : Controller
{
    private readonly HabitsRepository habitsRepository;

    public HabitsController(HabitsRepository habitsRepository)
    {
        this.habitsRepository = habitsRepository;
    }

    [HttpGet]
    public ViewResult New()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(NewHabitDto newHabit)
    {
        if (ModelState.IsValid)
        {
            var habit = new Habit(Guid.NewGuid(), newHabit.Title, DateTimeOffset.UtcNow)
            {
                Description = newHabit.Description
            };
            await this.habitsRepository.Save(habit);
            return RedirectToAction("New");
        }
        return View(newHabit);
    }
}

