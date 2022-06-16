namespace HabitTracker.Domain;

public interface HabitsRepository
{
    Task<IEnumerable<Habit>> GetAll();
    Task Save(Habit newHabit);
}

public class Habit
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; init; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ModifiedAt { get; init; }

    public Habit(Guid id, string title, DateTimeOffset createdAt)
    {
        if (id == Guid.Empty)
            throw new ArgumentException(nameof(id));
        if (string.IsNullOrWhiteSpace(title) || title.Length > 60)
            throw new ArgumentException(nameof(title));

        Id = id;
        Title = title;
        CreatedAt = createdAt;
        ModifiedAt = CreatedAt;
    }
}
