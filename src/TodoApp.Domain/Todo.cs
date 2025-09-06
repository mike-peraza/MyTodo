namespace TodoApp.Domain.Entities;

public class Todo
{
    public Guid Id { get; private set; }
    public string? Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    private Todo() { }

    public static Todo Create(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be empty", nameof(title));
        }

        return new Todo
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description ?? string.Empty,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsCompleted()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            CompletedAt = DateTime.UtcNow;
        }
    }

    public void MarkAsIncomplete()
    {
        if (IsCompleted)
        {
            IsCompleted = false;
            CompletedAt = null;
        }
    }

    public void UpdateDetails(string? title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        Title = title;
        Description = description ?? string.Empty;
    }
}
