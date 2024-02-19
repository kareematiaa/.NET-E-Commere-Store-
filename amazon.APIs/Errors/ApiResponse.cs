
namespace amazon.APIs.errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string  Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
                
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request, you have made",
                401 => "UnAuthorized",
                404 => "Resource Not Found",
                500 => "Errors are very bad , Go shift career",
                _ => null
            };
        }
    }
}
