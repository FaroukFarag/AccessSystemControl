namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Users;

public class SearchUsersResponse
{
    public int Total { get; set; }
    public IEnumerable<SearchUserResponse> Users { get; set; } = default!;
}
