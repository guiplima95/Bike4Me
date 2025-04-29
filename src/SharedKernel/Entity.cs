namespace SharedKernel;

public abstract class Entity
{
    public Guid Id { get; init; }

    public DateTime CreatedAt { get; protected set; }

    public DateTime UpdatedAt { get; protected set; }

    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}