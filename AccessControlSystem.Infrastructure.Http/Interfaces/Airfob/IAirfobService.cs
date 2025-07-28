using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Accounts;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.FloorLevels;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Schedules;
using AccessControlSystem.Infrastructure.Http.Interfaces.Airfob.Sites;

namespace AccessControlSystem.Infrastructure.Http.Interfaces.Airfob;

public interface IAirfobService :
    IAirfobSiteService,
    IAirfobScheduleService,
    IAirfobFloorLevelService,
    IAirfobAccountService
{
}
