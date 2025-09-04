using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.AccessGroups;
using AccessControlSystem.Application.Dtos.Cards;
using AccessControlSystem.Application.Dtos.Users;

namespace AccessControlSystem.Application.Dtos.Units;

public class UnitDto : BaseImageModelDto<int>
{
    public string Name { get; set; } = default!;
    public int Number { get; set; }
    public decimal Area { get; set; }
    public int CardNumber { get; set; }
    public int UsedCardsNumber { get; set; }
    public string? AssignedOwner { get; set; }
    public int SubscriptionId { get; set; }
    public string? SubscriptionCustomerName { get; set; }

    public IEnumerable<AccessGroupDto>? AccessGroups { get; set; }
    public IEnumerable<CardDto>? Cards { get; set; }
    public UserDto? Owner { get; set; }

}
