using System.Collections.Generic;
using System.Linq; // For Enumerable.Empty

namespace QuizAppPG.DTOs // Namespace is DTOs
{
    // Generic ServiceResult for operations that return data
    public class ServiceResult<T>
    {
        public T? Data { get; set; } // Nullable to allow for default(T) on failure
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; } // Nullable
        public IEnumerable<string>? Errors { get; set; } // Nullable, for multiple errors (e.g., validation)

        public static ServiceResult<T> Success(T data) => new ServiceResult<T> { Data = data, IsSuccess = true };
        public static ServiceResult<T> Failure(string errorMessage, string? errorDetails = null, IEnumerable<string>? errors = null) =>
            new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, Errors = errors ?? (errorDetails != null ? new[] { errorDetails } : null) };
    }

    // Non-generic ServiceResult for operations that only indicate success/failure
    public class ServiceResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; } // Nullable
        public IEnumerable<string>? Errors { get; set; } // Nullable

        public static ServiceResult Success() => new ServiceResult { IsSuccess = true };
        public static ServiceResult Failure(string errorMessage, string? errorDetails = null, IEnumerable<string>? errors = null) =>
            new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage, Errors = errors ?? (errorDetails != null ? new[] { errorDetails } : null) };
    }
}