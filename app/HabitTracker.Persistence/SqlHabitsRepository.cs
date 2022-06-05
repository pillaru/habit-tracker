using HabitTracker.Domain;

namespace HabitTracker.Persistence;

public class SqlHabitsRepository : HabitsRepository
{
    public Task<IEnumerable<Habit>> GetAll()
    {
        throw new NotImplementedException();
    }
}
