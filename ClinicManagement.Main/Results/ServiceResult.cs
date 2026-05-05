namespace ClinicManagement.Main.Results
{
    public class ServiceResult<T>
    {        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResult<T> Success(T data, string message = null, int statusCode = 200)
        {
            return new ServiceResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message,
                StatusCode = statusCode
            };
        }

        public static ServiceResult<T> Failure(string error, string message = null, int statusCode = 400)
        {
            return new ServiceResult<T>
            {
                IsSuccess = false,
                Error = error,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}