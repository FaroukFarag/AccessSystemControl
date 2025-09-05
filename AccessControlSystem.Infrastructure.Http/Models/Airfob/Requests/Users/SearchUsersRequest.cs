namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class SearchUsersRequest
{
    public IEnumerable<SearchUserFilter> Filters { get; set; } = default!;
}
