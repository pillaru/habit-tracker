namespace HabitTracker.Domain;

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
            throw new ArgumentException("Contains empty Guid", nameof(id));
        if (string.IsNullOrWhiteSpace(title) || title.Length > 60)
        {
            throw new ArgumentException(
                "Title must not be empty of longer than 60 characters long",
                nameof(title));
        }

        Id = id;
        Title = title;
        CreatedAt = createdAt;
        ModifiedAt = CreatedAt;
    }
}
