namespace QuizAppPG.Utilities
{
    public static class Constants
    {
        // IMPORTANT: Update this URL for your environment
        // For Android Emulator to Localhost Backend: "http://10.0.2.2:7207" (or your backend port)
        // For iOS Simulator/Device to Localhost Backend: "http://localhost:7207" (or your backend port)
        // For Windows Machine: "https://localhost:7207" (or your backend port)
        // Replace with your actual backend URL/IP for physical devices/deployment.
        public static string BackendUrl = "https://localhost:7207"; // Example for Windows. Change based on your setup.
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