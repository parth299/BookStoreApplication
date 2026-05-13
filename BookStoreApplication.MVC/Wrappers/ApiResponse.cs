namespace BookStoreApplication.MVC.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }

        public List<string>? Errors { get; set; }

        public int StatusCode { get; set; }

        public string? TraceId { get; set; }

        // ---------------- SUCCESS ----------------

        public static ApiResponse<T> SuccessResponse(
            T data,
            string message = "Success",
            int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> CreatedResponse(
            T data,
            string message = "Created")
        {
            return SuccessResponse(data, message, 201);
        }

        public static ApiResponse<T> NoContentResponse(
            string message = "No Content")
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = 204,
                Message = message
            };
        }

        // ---------------- ERROR ----------------

        public static ApiResponse<T> FailResponse(
            string message,
            int statusCode = 400,
            List<string>? errors = null,
            string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors ?? new List<string>(),
                TraceId = traceId
            };
        }

        public static ApiResponse<T> NotFound(
            string message,
            string? traceId = null)
        {
            return FailResponse(message, 404, null, traceId);
        }

        public static ApiResponse<T> BadRequest(
            string message,
            List<string>? errors = null,
            string? traceId = null)
        {
            return FailResponse(message, 400, errors, traceId);
        }
        public static ApiResponse<T> MessageResponse(
    string message,
    int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
