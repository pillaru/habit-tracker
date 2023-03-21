using HabitTracker.Controllers;
using HabitTracker.Domain;
using HabitTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.UnitTests.Controllers;

public sealed class HabitsControllerTests : IDisposable
{
    private readonly InMemoryHabitsRepository repository;
    private readonly HabitsController sut;

    public HabitsControllerTests()
    {
        repository = new InMemoryHabitsRepository();
        sut = new HabitsController(repository);
    }

    [Fact]
    public async Task Index_returns_all_habits()
    {
        Habit[] habits = new[]
        {
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow),
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow),
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow)
        };
        repository.AddRange(habits);
        ViewResult result = await sut.Index().ConfigureAwait(false);
        Assert.Equal(result.Model, habits);
    }

    [Fact]
    public async Task Details_returns_NotFound()
    {
        var id = new Guid("75c6e184-0a07-4a27-b5a9-e900df0dc2ce");
        ActionResult result = await sut.Details(id).ConfigureAwait(false);
        _ = Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Details_returns_Habit()
    {
        var habit = new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow);
        await repository.Save(habit).ConfigureAwait(false);
        ActionResult result = await sut.Details(habit.Id).ConfigureAwait(false);
        ViewResult actualResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(actualResult.Model, habit);
    }

    [Fact]
    public async Task Delete_redirects_to_Index()
    {
        var habit = new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow);
        await repository.Save(habit).ConfigureAwait(false);
        ActionResult result = await sut.Delete(habit.Id).ConfigureAwait(false);
        RedirectToActionResult redirection = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirection.ActionName);
        Habit? deleted = await repository.GetById(habit.Id).ConfigureAwait(false);
        Assert.Null(deleted);
    }

    [Fact]
    public void New_returns_ViewResult()
    {
        ViewResult result = sut.New();
        _ = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task New_redirects_to_Index_after_saving()
    {
        var habitDto = new NewHabitDto("title", "description");
        IActionResult result = await sut.New(habitDto).ConfigureAwait(false);
        RedirectToActionResult redirection = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirection.ActionName);
        IEnumerable<Habit> habits = await repository.GetAll().ConfigureAwait(false);
        Habit? habit = habits.FirstOrDefault(
                x => x.Title == "title" && x.Description == "description");
        Assert.NotNull(habit);
    }

    [Fact]
    public async Task New_doesnt_save_with_invalid_ModelState()
    {
        var habitDto = new NewHabitDto("title", "description");
        sut.ModelState.AddModelError("title", "an error");
        IActionResult result = await sut.New(habitDto).ConfigureAwait(false);
        ViewResult view = Assert.IsType<ViewResult>(result);
        Assert.Equal(habitDto, view.Model);
        IEnumerable<Habit> habits = await repository.GetAll().ConfigureAwait(false);
        Habit? habit = habits.FirstOrDefault(
                x => x.Title == "title" && x.Description == "description");
        Assert.Null(habit);
    }

    public void Dispose()
    {
        sut.Dispose();
    }
}

