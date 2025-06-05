namespace QuizAppPG.Services.Local
{
    public class NavigationService : INavigationService
    {
        public Task GoToAsync(string route, bool animate = true)
        {
            return Shell.Current.GoToAsync(route, animate);
        }

        public Task GoToAsync(string route, IDictionary<string, object> parameters, bool animate = true)
        {
            return Shell.Current.GoToAsync(route, animate, parameters);
        }

        public Task PopAsync(bool animate = true)
        {
            return Shell.Current.GoToAsync("..", animate);
        }

        public Task PopToRootAsync(bool animate = true)
        {
            return Shell.Current.GoToAsync("///MainPage", animate);
        }
    }
}