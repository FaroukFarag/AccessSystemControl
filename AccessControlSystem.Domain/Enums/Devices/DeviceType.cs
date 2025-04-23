using System.ComponentModel;

namespace AccessControlSystem.Domain.Enums.Devices;

public enum DeviceType
{
    [Description("Airfob Edge Reader")]
    AirfobEdgeReader = 1,
    [Description("Airfob Edge Reader Ultimate")]
    AirfobEdgeReaderUltimate,
    [Description("Airfob Tag")]
    AirfobTag,
    [Description("Airfob Patch")]
    AirfobPatch,
    [Description("Suprema X-Station 2")]
    SupremaXStation2,
    [Description("Wireless Door Locks")]
    WirelessDoorLocks
}
