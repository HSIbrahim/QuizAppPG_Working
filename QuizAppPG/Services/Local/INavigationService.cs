namespace QuizAppPG.Services.Local
{
    public interface INavigationService
    {
        Task GoToAsync(string route, bool animate = true);
        Task GoToAsync(string route, IDictionary<string, object> parameters, bool animate = true);
        Task PopAsync(bool animate = true);
        Task PopToRootAsync(bool animate = true);
    }
}