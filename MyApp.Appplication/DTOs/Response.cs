
namespace MyApp.Application.DTOs
{
    public class Response<T>
    {
        public Status Status { get; set; } = new Status();
        public T? Result { get; set; }

        public static Response<T> Success(T result, string message = "")
        {
            return new Response<T>
            {
                Status = new Status { Success = true, Message = message },
                Result = result
            };
        }

        public static Response<T> Fail(string message, string errorCode = "")
        {
            return new Response<T>
            {
                Status = new Status { Success = false, Message = message, ErrorCode = errorCode }
            };
        }
    }

    public class Status
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
    }
}
