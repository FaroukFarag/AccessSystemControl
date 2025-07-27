namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Devices;

public class Tna
{
    public string Mode { get; set; } = default!;
    public IEnumerable<TnaCode> Codes { get; set; } = default!;
}
