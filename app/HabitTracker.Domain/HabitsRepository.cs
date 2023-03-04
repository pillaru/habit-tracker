namespace HabitTracker.Domain;

public interface HabitsRepository
{
    Task<IEnumerable<Habit>> GetAll();
    Task<Habit?> GetById(Guid id);
    Task Save(Habit newHabit);
}
