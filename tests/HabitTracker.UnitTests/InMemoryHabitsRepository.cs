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
        Habit? item = items.SingleOrDefault(x => x.Id == id);
        if (item != null)
        {
            bool _ = items.Remove(item);
        }
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Habit>> GetAll()
    {
        return Task.FromResult(items.AsEnumerable());
    }

    public Task<Habit?> GetById(Guid id)
    {
        Habit? item = items.SingleOrDefault(x => x.Id == id);
        return Task.FromResult(item);
    }

    public Task Save(Habit habit)
    {
        items.Add(habit);
        return Task.CompletedTask;
    }
}
