// Services/Local/IDialogService.cs
using Microsoft.Maui.Controls; // For Keyboard

namespace QuizAppPG.Services.Local
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string cancel = "OK");
        Task<bool> ShowConfirmAsync(string title, string message, string accept = "Yes", string cancel = "No");
        Task<string?> ShowPromptAsync(string title, // Corrected return type to nullable
                                     string message,
                                     string accept = "OK",
                                     string cancel = "Cancel",
                                     string? placeholder = null,
                                     int maxLength = -1,
                                     Keyboard keyboard = default);
    }
}