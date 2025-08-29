namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class ActivateUsersRequest
{
    public IEnumerable<ActivateUserRequest> Users { get; set; } = default!;
}
