namespace HabitTracker.Domain;

public interface HabitsRepository
{
    Task Delete(Guid id);
    Task<IEnumerable<Habit>> GetAll();
    Task<Habit?> GetById(Guid id);
    Task Save(Habit newHabit);
}
