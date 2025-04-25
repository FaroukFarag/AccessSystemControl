using AccessControlSystem.Application.Dtos.Units;
using AccessControlSystem.Application.Interfaces.Shared;
using AccessControlSystem.Application.Interfaces.Units;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Constants.Subscriptions;
using AccessControlSystem.Domain.Interfaces.Repositories.Units;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Units;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Units;

public class UnitService(
    IUnitRepository repository,
IUnitOfWork unitOfWork,
    IMapper mapper,
    IImageService imageService) :
    BaseService<Unit, UnitDto, int>(repository, unitOfWork, mapper),
    IUnitService
{
    private readonly IImageService _imageService = imageService;

    public override async Task<UnitDto> CreateAsync(UnitDto unitDto)
    {
        unitDto.ImagePath = await _imageService
            .SaveImageAsync(unitDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.CreateAsync(unitDto);
    }

    public override async Task<UnitDto> GetAsync(int id)
    {
        var unit = await base.GetAsync(id);

        if (!string.IsNullOrEmpty(unit.ImagePath) && File.Exists(unit.ImagePath))
        {
            var imageBytes = await File.ReadAllBytesAsync(unit.ImagePath);

            unit.ImageEncode = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
        }

        return unit;
    }

    public override async Task<IEnumerable<UnitDto>> GetAllAsync()
    {
        var units = await base.GetAllAsync();

        foreach (var unit in units)
        {
            if (!string.IsNullOrEmpty(unit.ImagePath) && File.Exists(unit.ImagePath))
            {
                var imageBytes = await File.ReadAllBytesAsync(unit.ImagePath);

                unit.ImageEncode = $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
            }
        }

        return units;
    }

    public override async Task<UnitDto> Update(UnitDto newUnitDto)
    {
        var unit = await GetAsync(newUnitDto.Id);

        _imageService.DeleteImageAsync(unit.ImagePath!);

        newUnitDto.ImagePath = await _imageService
            .SaveImageAsync(newUnitDto.ImageFile, SubscriptionConstants.SubFolder);

        return await base.Update(newUnitDto);
    }

    public override async Task<UnitDto> Delete(int id)
    {
        var unit = await GetAsync(id);

        _imageService.DeleteImageAsync(unit.ImagePath!);

        return await base.Delete(id);
    }
}
