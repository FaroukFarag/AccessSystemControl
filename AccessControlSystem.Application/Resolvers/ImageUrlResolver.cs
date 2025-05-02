using AccessControlSystem.Application.Configurations;
using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Models.Abstraction;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace AccessControlSystem.Application.Resolvers;

public class ImageUrlResolver(IOptions<ImageSettings> settings) : IValueResolver<BaseImageModel<int>, BaseImageModelDto<int>, string?>
{
    private readonly ImageSettings _settings = settings.Value;

    public string? Resolve(BaseImageModel<int> source, BaseImageModelDto<int> destination, string? destMember, ResolutionContext context)
    {
        if (string.IsNullOrWhiteSpace(source.ImagePath))
            return null;

        return $"{_settings.BaseUrl.TrimEnd('/')}/{source.ImagePath.TrimStart('/')}";
    }
}
