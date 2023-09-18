
namespace filament.api.v1;

public class ApiResponse<T>
{
    public ApiResponse(T data)
    {
        Data = data;
    }

    public T Data { get; set; }
}