using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Application.Interfaces.Devices;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.Devices;
using AutoMapper;

namespace AccessControlSystem.Application.Services.Devices;

public class DeviceService(
    IDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<Device, DeviceDto, int>(repository, unitOfWork, mapper), IDeviceService
{
}
