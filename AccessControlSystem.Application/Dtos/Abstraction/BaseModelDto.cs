namespace AccessControlSystem.Application.Dtos.Abstraction;

public abstract class BaseModelDto<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; } = default!;
}
