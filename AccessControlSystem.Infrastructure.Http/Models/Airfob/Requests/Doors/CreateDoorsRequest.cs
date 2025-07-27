namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Doors;

public class CreateDoorsRequest
{
    public IEnumerable<CreateDoorRequest> Doors { get; set; } = default!;
}
