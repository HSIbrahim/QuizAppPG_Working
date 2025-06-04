using System; // For Guid
using System.Collections.Generic;
using QuizAppPG.Utilities; // Corrected: Points to Utilities for DifficultyLevel and QuestionType

namespace QuizAppPG.DTOs // Namespace is DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public DifficultyLevel Difficulty { get; set; }
        public QuestionType Type { get; set; }
        public int QuizCategoryId { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty; // Added CorrectAnswer for solo quiz feedback (if backend sends it)
    }

    public class SubmitAnswerDto
    {
        public int QuestionId { get; set; }
        public string SubmittedAnswer { get; set; } = string.Empty;
        public Guid GameSessionId { get; set; } // If it's an online session
    }

    public class AnswerResultDto
    {
        public bool IsCorrect { get; set; }
        public int PointsAwarded { get; set; }
        public int CurrentScore { get; set; }
    }
}