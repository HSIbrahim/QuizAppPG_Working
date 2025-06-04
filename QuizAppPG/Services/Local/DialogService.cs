// Services/Local/DialogService.cs
using Microsoft.Maui.Controls; // For Keyboard, Application, Window

namespace QuizAppPG.Services.Local
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string title, string message, string cancel = "OK")
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (currentPage != null)
            {
                return currentPage.DisplayAlert(title, message, cancel);
            }
            return Task.CompletedTask;
        }

        public async Task<bool> ShowConfirmAsync(string title, string message, string accept = "Yes", string cancel = "No")
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (currentPage != null)
            {
                string? result = await currentPage.DisplayActionSheet(title, cancel, accept, message);
                return result == accept;
            }
            return false;
        }

        public Task<string?> ShowPromptAsync(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1, Keyboard keyboard = default)
        {
            var currentPage = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (currentPage != null)
            {
                return currentPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard);
            }
            return Task.FromResult<string?>(null); // Corrected return type to nullable
        }
    }
}