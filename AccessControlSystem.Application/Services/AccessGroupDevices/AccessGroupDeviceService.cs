using AccessControlSystem.Application.Dtos.AccessGroupDevices;
using AccessControlSystem.Application.Interfaces.AccessGroupDevices;
using AccessControlSystem.Application.Services.Abstraction;
using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupDevices;
using AccessControlSystem.Domain.Interfaces.UnitOfWork;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AutoMapper;

namespace AccessControlSystem.Application.Services.AccessGroupDevices;

public class AccessGroupDeviceService(
    IAccessGroupDeviceRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) :
    BaseService<
        AccessGroupDevice,
        AccessGroupDeviceDto,
        (int AccessGroupId, int DeviceId)>(
        repository, unitOfWork, mapper),
    IAccessGroupDeviceService
{
}
