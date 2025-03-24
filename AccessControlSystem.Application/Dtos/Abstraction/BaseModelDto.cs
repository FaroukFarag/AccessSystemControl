namespace AccessControlSystem.Application.Dtos.Abstraction;

public class BaseModelDto<TPrimaryKey>
{
    public TPrimaryKey Id { get; set; } = default!;
}
