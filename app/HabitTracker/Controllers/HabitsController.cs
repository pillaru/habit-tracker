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
    public async Task<ViewResult> Index()
    {
        var habits = await this.habitsRepository.GetAll();
        return View(habits);
    }

    [HttpGet]
    public async Task<ActionResult> Details(Guid id)
    {
        var habit = await habitsRepository.GetById(id);
        if (habit == null) return NotFound();
        return View(habit);
    }

    [HttpGet]
    public ActionResult Delete(Guid id)
    {
        return RedirectToAction("Details", new { id });
        // var result = await this.habitsRepository.Delete(id);
        // return result
        //     .Select(_ => RedirectToAction("Index"))
        //     .SelectError(_ => NotFound())
        //     .BiFold();
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
            return RedirectToAction("Index");
        }
        return View(newHabit);
    }
}

