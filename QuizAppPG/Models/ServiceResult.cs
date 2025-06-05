namespace QuizAppPG.DTOs 
{
    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; } 
        public IEnumerable<string>? Errors { get; set; } 

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { Data = data, IsSuccess = true };
        public static ServiceResult<T> Failure(string errorMessage, string? errorDetails = null, IEnumerable<string>? errors = null) =>
            new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, Errors = errors ?? (errorDetails != null ? new[] { errorDetails } : null) };
    }
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; } 
        public IEnumerable<string>? Errors { get; set; }

        public static ServiceResult Success() => new ServiceResult { IsSuccess = true };
        public static ServiceResult Failure(string errorMessage, string? errorDetails = null, IEnumerable<string>? errors = null) =>
            new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage, Errors = errors ?? (errorDetails != null ? new[] { errorDetails } : null) };
    }
}