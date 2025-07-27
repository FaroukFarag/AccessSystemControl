namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Doors;

public class GetDoorsResponse
{
    public int Total { get; set; }
    public IEnumerable<GetDoorResponse> Doors { get; set; } = default!;
}
