namespace HabitTracker.Domain;

public interface HabitsRepository
{
    Task<IEnumerable<Habit>> GetAll();
}

public class Habit
{
    public string Title { get; private set; }
    public string Description { get; init; }

    public Habit(string title)
    {
        Title = title;
    }
}
