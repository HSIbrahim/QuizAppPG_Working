namespace QuizAppPG.Services.Local
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string cancel = "OK");
        Task<bool> ShowConfirmAsync(string title, string message, string accept = "Yes", string cancel = "No");
        Task<string?> ShowPromptAsync(string title,
                                     string message,
                                     string accept = "OK",
                                     string cancel = "Cancel",
                                     string? placeholder = null,
                                     int maxLength = -1,
                                     Keyboard keyboard = default);
    }
}