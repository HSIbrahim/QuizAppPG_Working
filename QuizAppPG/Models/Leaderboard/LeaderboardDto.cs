namespace QuizAppPG.DTOs // Namespace is DTOs
{
    public class LeaderboardEntryDto
    {
        public string Username { get; set; } = string.Empty;
        public int TotalScore { get; set; }
    }
}