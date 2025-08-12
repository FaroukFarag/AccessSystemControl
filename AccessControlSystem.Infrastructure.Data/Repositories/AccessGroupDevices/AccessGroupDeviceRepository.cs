using AccessControlSystem.Domain.Interfaces.Repositories.AccessGroupDevices;
using AccessControlSystem.Domain.Interfaces.Specifications.Absraction;
using AccessControlSystem.Domain.Models.AccessGroupDevices;
using AccessControlSystem.Domain.Services.Shared;
using AccessControlSystem.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace AccessControlSystem.Infrastructure.Data.Repositories.AccessGroupDevices;

public class AccessGroupDeviceRepository(
    AccessControlDbContext context,
    ISpecificationCombiner<AccessGroupDevice> specificationCombiner,
    IQueryBuilder<AccessGroupDevice> queryBuilder,
    IEntityFinder<AccessGroupDevice,
        (int AccessGroupId, int DeviceId)> entityFinder,
    IPaginationService paginationService,
    ILogger<AccessGroupDeviceRepository> logger) :
    BaseRepository<AccessGroupDevice, (int AccessGroupId, int DeviceId)>(context,
        specificationCombiner, queryBuilder, entityFinder, paginationService, logger),
    IAccessGroupDeviceRepository
{
}
