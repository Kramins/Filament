
namespace filament.api.v1;

public class ApiResponse<T>
{
    public ApiResponse(T data)
    {
        Data = data;
    }

    public T Data { get; set; }

    public static implicit operator ApiResponse<T>(T data)
    {
        return ApiResponse<T>.Create(data);
    }

    public static ApiResponse<T> Create(T data)
    {
        var response = new ApiResponse<T>(data);
        return response;
    }
}