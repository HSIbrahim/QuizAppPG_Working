using System.ComponentModel.DataAnnotations;

namespace QuizAppPG.DTOs
{
    public class CreateGameSessionDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Välj en giltig kategori.")]
        public int QuizCategoryId { get; set; }

        public DifficultyLevel Difficulty { get; set; }
    }

    public class GameSessionDetailsDto
    {
        public Guid Id { get; set; }
        public string HostUsername { get; set; } = string.Empty;
        public int QuizCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DifficultyLevel Difficulty { get; set; } 
        public DateTime StartTime { get; set; }
        public bool IsActive { get; set; }
        public List<GameSessionPlayerDto> Players { get; set; } = new List<GameSessionPlayerDto>();
    }

    public class GameSessionPlayerDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public int Score { get; set; }
        public bool IsHost { get; set; }
    }
}