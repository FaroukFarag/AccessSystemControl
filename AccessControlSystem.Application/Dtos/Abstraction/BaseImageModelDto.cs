using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Application.Dtos.Abstraction;

public abstract class BaseImageModelDto<T> : BaseModelDto<T>
{
    public string? ImagePath { get; set; }
    public string? ImageEncode { get; set; }
    public IFormFile ImageFile { get; set; } = default!;
}
