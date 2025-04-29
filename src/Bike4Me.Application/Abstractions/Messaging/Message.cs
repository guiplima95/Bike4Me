namespace Bike4Me.Application.Abstractions.Messaging;

public abstract class Message
{
    public Message()
    {
        Id = Guid.NewGuid();
        MessageType = GetType().Name;
        Timestamp = DateTime.UtcNow;
    }

    public Guid Id { get; protected set; }

    public string MessageType { get; protected set; }

    public DateTime Timestamp { get; protected set; }

    public override string ToString()
    {
        return $"Id=[{Id}] | Type=[{MessageType}]";
    }

    public override bool Equals(object obj)
    {
        var compareTo = obj as Message;

        if (ReferenceEquals(this, compareTo))
        {
            return true;
        }

        if (compareTo is null)
        {
            return false;
        }

        return Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}