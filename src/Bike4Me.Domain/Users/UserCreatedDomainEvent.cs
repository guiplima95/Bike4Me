using SharedKernel;

namespace Bike4Me.Domain.Users;

public sealed record UserCreatedDomainEvent(Guid UserId) : IDomainEvent;