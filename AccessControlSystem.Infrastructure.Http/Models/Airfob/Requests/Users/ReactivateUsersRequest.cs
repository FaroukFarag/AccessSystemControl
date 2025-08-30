namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class ReactivateUsersRequest
{
    public IEnumerable<ReactivateUserRequest> Users { get; set; } = default!;
}
