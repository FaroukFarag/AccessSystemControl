namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Accounts;

public class GetSelfAccountResponse
{
    public IEnumerable<AccountResponse> Accounts { get; set; } = default!;
}
