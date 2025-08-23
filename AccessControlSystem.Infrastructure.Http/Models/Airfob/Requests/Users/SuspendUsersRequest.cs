namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class SuspendUsersRequest
{
    public IEnumerable<int> Ids { get; set; } = default!;
    public string CertifyBy { get; set; } = "none";
    public bool UseSiteTemplate { get; set; } = true;
}
