namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Users;

public class CreateUsersRequest
{
    public IEnumerable<CreateUserRequest> Users { get; set; } = default!;
}
