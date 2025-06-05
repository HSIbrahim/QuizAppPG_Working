namespace QuizAppPG.DTOs
{
    public class ErrorDto
    {
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? Code { get; set; }
    }
}