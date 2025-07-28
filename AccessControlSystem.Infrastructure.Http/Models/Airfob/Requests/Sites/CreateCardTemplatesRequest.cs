namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Requests.Sites;

public class CreateCardTemplatesRequest
{
    public IEnumerable<CreateCardTemplateRequest> Templates { get; set; } = default!;
}
