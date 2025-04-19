using AccessControlSystem.Application.Dtos.Abstraction;
using AccessControlSystem.Domain.Enums.Subscriptions;
using Microsoft.AspNetCore.Http;

namespace AccessControlSystem.Application.Dtos.Subscriptions;

public class SubscriptionDto : BaseModelDto<int>
{
    public string? ImagePath { get; set; }
    public string? ImageEncode { get; set; }
    public string CustomerName { get; set; } = default!;
    public SubscriptionType SubscriptionType { get; set; }
    public int DeviceNumber { get; set; }
    public decimal PaymentPerMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Note { get; set; }

    public IFormFile ImageFile { get; set; } = default!;
}
