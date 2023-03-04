namespace HabitTracker.Domain;

public interface HabitsRepository
{
    Task<IEnumerable<Habit>> GetAll();
    Task Save(Habit newHabit);
}
