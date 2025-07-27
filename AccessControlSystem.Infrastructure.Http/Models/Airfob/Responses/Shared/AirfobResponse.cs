namespace AccessControlSystem.Infrastructure.Http.Models.Airfob.Responses.Shared;

public class AirfobResponse<T>
    where T : class
{
    public bool Succeeded { get; set; }
    public T ResultData { get; set; } = default!;

    public static AirfobResponse<T> CreateSuccessResponse(T resultData)
    {
        return new AirfobResponse<T>
        {
            Succeeded = true,
            ResultData = resultData
        };
    }

    public static AirfobResponse<T> CreateFailResponse()
    {
        return new AirfobResponse<T>
        {
            Succeeded = false
        };
    }
}
