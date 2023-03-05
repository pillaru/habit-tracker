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
        IEnumerable<Habit> habits = await this.habitsRepository.GetAll()
            .ConfigureAwait(false);
        return View(habits);
    }

    [HttpGet]
    public async Task<ActionResult> Details(Guid id)
    {
        Habit? habit = await habitsRepository.GetById(id).ConfigureAwait(false);
        return habit == null ? NotFound() : View(habit);
    }

    [HttpGet]
    public async Task<ActionResult> Delete(Guid id)
    {
        await habitsRepository.Delete(id).ConfigureAwait(false);
        return RedirectToAction("Index");
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
        ArgumentNullException.ThrowIfNull(newHabit);
        if (ModelState.IsValid)
        {
            var habit = new Habit(Guid.NewGuid(), newHabit.Title, DateTimeOffset.UtcNow)
            {
                Description = newHabit.Description
            };
            await this.habitsRepository.Save(habit).ConfigureAwait(false);
            return RedirectToAction("Index");
        }
        return View(newHabit);
    }
}

