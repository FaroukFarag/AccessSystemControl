using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.AccessGroups;
using System.Text.Json;

namespace AccessControlSystem.Application.Dtos.Units;

public class CreateUnitDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public int SubscriptionId { get; set; }
    public string? AccessGroupsJson { get; set; }

    public IEnumerable<AccessGroupDto>? AccessGroups
    {
        get
        {
            if (string.IsNullOrEmpty(AccessGroupsJson))
                return _accessGroups;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<AccessGroupDto>>(AccessGroupsJson, options);
        }
        set
        {
            _accessGroups = value;
        }
    }

    private IEnumerable<AccessGroupDto>? _accessGroups;

}
