namespace AccessControlSystem.Application.Common.Mappings;

public static class DeviceTrafficCodeMapper
{
    private static readonly Dictionary<string, string> CodeDescriptions = new()
    {
        // Mobile Authentication
        {"10080", "Successful device authentication on mobile"},
        {"11000", "The Mobile card not activated (before server connection, expired, suspended, recalled)"},
        {"11100", "The Mobile card validity period is invalid"},
        {"10013", "[Airfob Patch Only] If the power is low, authentication succeeds because it is after NFC output"},
        {"11813", "Mobile network disconnected"},
        {"11814", "When the device cannot be accessed due to lack of permission"},
        {"11815", "When it is a holiday with access rights"},
        {"11816", "When access is not permitted"},
        {"11818", "Unregistered device"},
        {"11819", "If the period is not allowed for the access level"},
        {"11820", "Request timeout after authentication attempt connection"},
        {"11825", "If the T&A code is not entered when authenticating to a device with T&A settings"},
        
        // RF Card Authentication
        {"20001", "Authentication successful (RF Card)"},
        {"20002", "Offline authentication successful (RF Card)"},
        {"21001", "Authentication failed (RF Card)"},
        {"21002", "Offline authentication failed (RF Card)"},
        {"21003", "Inactive card (RF Card does not exist)"},
        {"21004", "Holiday error (RF Card)"},
        {"21005", "Time error (RF Card)"},
        {"21006", "Level error (RF Card)"},
        {"21007", "Wrong period (RF Card)"},
        {"21009", "Access denied (Need TnA)"},
        
        // Web QR Authentication
        {"20003", "Authentication successful (Web QR)"},
        {"21010", "Authentication failed (Web QR)"},
        {"21012", "Inactive card (Web QR card does not exist)"},
        {"21013", "Holiday error (Web QR)"},
        {"21014", "Time error (Web QR)"},
        {"21015", "Level error (Web QR)"},
        {"21016", "Wrong period (Web QR)"},
        {"21018", "Authentication failed (QR)"},
        
        // Mobile QR Authentication
        {"20004", "Authentication successful (Mobile QR)"},
        {"20006", "Authentication successful (Mobile offline)"},
        {"21019", "Authentication failed (Mobile QR)"},
        {"21021", "Inactive card (Mobile QR card does not exist)"},
        {"21022", "Holiday error (Mobile QR)"},
        {"21023", "Time error (Mobile QR)"},
        {"21024", "Level error (Mobile QR)"},
        {"21025", "Wrong period (Mobile QR)"},
        
        // Apple Wallet Authentication
        {"20005", "Authentication successful (Apple Wallet)"},
        {"21027", "Authentication failed (Apple Wallet)"},
        {"21029", "Inactive card (Apple Wallet)"},
        {"21030", "Holiday error (Apple Wallet)"},
        {"21031", "Time error (Apple Wallet)"},
        {"21032", "Level error (Apple Wallet)"},
        {"21033", "Wrong period (Apple Wallet)"},
        
        // Upgrade
        {"20101", "Upgrade Successful"},
        {"21101", "Upgrade failed"},
        
        // Device Status
        {"22001", "Start the device"},
        {"22002", "Restart your device"},
        {"22003", "Network connection"},
        {"22004", "Network disconnected"},
        
        // Door and Input Events
        {"22101", "Door Open"},
        {"22102", "Door Locked"},
        {"22103", "TTL input 0 port on"},
        {"22104", "TTL input 0 port off"},
        {"22105", "TTL input 1 port on"},
        {"22106", "TTL input 1 port off"},
        {"22107", "Tamper on"},
        {"22108", "Tamper off"},
        
        // Device Management
        {"22201", "Device registration"},
        {"22202", "Delete device"},
        {"22301", "Door open"},
        {"22302", "Door closed"},
        {"22303", "Manually Opened"},
        {"22304", "Manually Closed"},
        {"22305", "Device settings"},
        {"22306", "Get device settings"},
        {"22307", "Get device information"},
        {"22308", "Start upgrade"},
        {"22309", "Reset settings"},
        {"2230A", "Reset network exclusions"},
        {"2230B", "Restart your device"},
        {"22313", "Read device rfcard"},
        
        // Link Pass and PIN Authentication
        {"30001", "Authentication successful (Link Pass)"},
        {"30002", "Authentication successful (One-time PIN)"},
        {"30003", "Authentication successful (RF, PIN)"},
        {"31001", "Authentication failed (Link Pass)"},
        {"31003", "Inactive card (Web Link card does not exist)"},
        {"31004", "Holiday error (Link Pass)"},
        {"31005", "Time error (Link Pass)"},
        {"31006", "Level error (Link Pass)"},
        {"31007", "Wrong period (Link Pass)"},
        {"31009", "Authentication failed (Keypad PIN)"},
        {"31010", "Authentication failed (One-time PIN)"},
        {"32004", "Network disconnected (Link Pass)"}
    };

    public static string GetTrafficTypeDescription(string code)
    {
        if (int.TryParse(code, out int numericCode))
        {
            if (numericCode >= 11002 && numericCode <= 11804)
                return "Internal Error";
            if (numericCode >= 12000 && numericCode <= 12999)
                return "Same as existing authentication success (network connection x)";
            if (numericCode >= 13000 && numericCode <= 13999)
                return "Same as existing authentication failure (network connection x)";
        }

        return CodeDescriptions.TryGetValue(code, out string? description) ? description : $"Unknown code: {code}";
    }
}
