using HabitTracker.Controllers;
using HabitTracker.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.UnitTests.Controllers;

public class HabitsControllerTests
{
    [Fact]
    public async Task Index_returns_all_habits()
    {
        Habit[] habits = new[]
        {
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow),
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow),
            new Habit(Guid.NewGuid(), "title", DateTimeOffset.UtcNow)
        };
        var repository = new InMemoryHabitsRepository();
        repository.AddRange(habits);
        using var sut = new HabitsController(repository);
        ViewResult result = await sut.Index().ConfigureAwait(false);
        Assert.Equal(result.Model, habits);
    }
}

