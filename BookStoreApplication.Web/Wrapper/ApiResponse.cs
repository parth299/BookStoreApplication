namespace BookStoreApplication.Web.Wrapper;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
    public T? Data { get; set; }
    public IReadOnlyList<string>? Errors { get; set; }

    public static ApiResponse<T> Success(int statusCode, string message, T? data)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Failure(int statusCode, string message, IReadOnlyList<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            StatusCode = statusCode,
            Message = message,
            Errors = errors
        };
    }
}
