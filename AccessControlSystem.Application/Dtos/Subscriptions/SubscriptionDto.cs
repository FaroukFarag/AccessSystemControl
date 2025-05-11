using AccessControlSystem.Application.Common.Utilities;
using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Application.Dtos.Devices;
using AccessControlSystem.Domain.Enums.Subscriptions;

namespace AccessControlSystem.Application.Dtos.Subscriptions;

public class SubscriptionDto : BaseImageModelDto<int>
{
    public string CustomerName { get; set; } = default!;
    public SubscriptionType SubscriptionType { get; set; }
    public string? SubscriptionTypeName { get; set; }
    public int AdminNumber { get; set; }
    public int DeviceNumber { get; set; }
    public int CardNumber { get; set; }
    public int MonthNumber { get; set; }
    public int UsedAdmins { get; set; }
    public int UsedDevices { get; set; }
    public int UsedCards { get; set; }
    public decimal PaymentPerMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate => StartDate.AddMonths(MonthNumber);
    public string? Note { get; set; }
    public string RenewalInfo => RenewalCalculator.GetRenewalInfo(EndDate);

    public IEnumerable<DeviceDto>? Devices { get; set; }
}
