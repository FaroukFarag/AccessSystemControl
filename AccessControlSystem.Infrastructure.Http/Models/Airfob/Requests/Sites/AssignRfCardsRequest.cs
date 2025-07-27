namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class AssignRfCardsRequest
{
    public int SiteId { get; set; }
    public IEnumerable<AssignRfCardRequest> Cards { get; set; } = default!;
}
