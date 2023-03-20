using HabitTracker.Controllers;
using HabitTracker.Domain;
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
        NotFoundResult _ = Assert.IsType<NotFoundResult>(result);
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
    
    public void Dispose()
    {
        sut.Dispose();
    }
}

