using HabitTracker.Domain;

namespace HabitTracker.UnitTests;

internal class InMemoryHabitsRepository : HabitsRepository
{
    private readonly List<Habit> items = new();

    public void AddRange(Habit[] habits)
    {
        items.AddRange(habits);
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Habit>> GetAll()
    {
        return Task.FromResult(items.AsEnumerable());
    }

    public Task<Habit?> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Save(Habit habit)
    {
        throw new NotImplementedException();
    }
}
