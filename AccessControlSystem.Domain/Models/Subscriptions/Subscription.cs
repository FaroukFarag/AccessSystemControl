﻿using AccessControlSystem.Domain.Enums.Subscriptions;
using AccessControlSystem.Domain.Models.Abstraction;
using AccessControlSystem.Domain.Models.Cards;
using AccessControlSystem.Domain.Models.Devices;
using AccessControlSystem.Domain.Models.Users;

namespace AccessControlSystem.Domain.Models.Subscriptions;

public class Subscription : BaseModel<int>
{
    public string ImagePath { get; set; } = default!;
    public string CustomerName { get; set; } = default!;
    public SubscriptionType SubscriptionType { get; set; }
    public int DeviceNumber { get; set; }
    public decimal PaymentPerMonth { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string Note { get; set; } = default!;

    public IEnumerable<User> Users { get; set; } = default!;
    public IEnumerable<Device> Devices { get; set; } = default!;
    public IEnumerable<Card> Cards { get; set; } = default!;
}
