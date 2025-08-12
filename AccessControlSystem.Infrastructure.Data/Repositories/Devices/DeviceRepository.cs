using AccessControlSystem.Domain.Interfaces.Repositories.Devices;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.Devices;

public class DeviceRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<Device> specificationCombiner,
    IQueryBuilder<Device> queryBuilder,
    IEntityFinder<Device, int> entityFinder,
    IPaginationService paginationService,
    ILogger<DeviceRepository> logger) :
    BaseRepository<Device, int>(context, specificationCombiner, queryBuilder,
        entityFinder, paginationService, logger), IDeviceRepository
{
}
