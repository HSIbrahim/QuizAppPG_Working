using System.ComponentModel.DataAnnotations;

namespace QuizAppPG.DTOs 
{
    public class UserProfileDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int TotalScore { get; set; }
    }

    public class UpdateProfileDto
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Användarnamn måste vara mellan 3 och 50 tecken.")]
        public string? NewUsername { get; set; }

        [EmailAddress(ErrorMessage = "Ogiltig e-postadress.")]
        public string? NewEmail { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Nuvarande lösenord är obligatoriskt.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nytt lösenord är obligatoriskt.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Nytt lösenord måste vara minst 6 tecken långt.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Lösenord och bekräftelselösenord matchar inte.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}