using AccessControlSystem.Common.Interfaces.Subscriptions;

namespace AccessControlSystem.Domain.Models.Abstraction;

public abstract class BaseModel<TPrimaryKey> : IAuditable
{
    public TPrimaryKey Id { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
