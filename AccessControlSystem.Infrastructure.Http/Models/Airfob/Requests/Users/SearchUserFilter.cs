namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class SearchUserFilter
{
    public string Field { get; set; } = default!;
    public new string Equals { get; set; } = default!;
    public string Gte { get; set; } = default!;
    public string Lte { get; set; } = default!;
    public string contains { get; set; } = default!;
}
