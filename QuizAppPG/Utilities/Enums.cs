namespace QuizAppPG.Utilities
{
    public static class Constants
    {
        public static string BackendUrl = "https://localhost:7207";
        public static string SignalRHubUrl = $"{BackendUrl}/gamehub";
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    public enum QuestionType
    {
        MultipleChoice,
        TrueFalse
    }
}