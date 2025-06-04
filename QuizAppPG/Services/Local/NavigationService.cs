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
            // This is a common way to pop to root using Shell.
            // Shell.Current.GoToAsync("//MainPage"); // Example if MainPage is a tab root
            // Or use an absolute path for specific root if needed
            // For general pop to root, navigate to the route leading to the root most directly.
            // Simplified: pop until current stack is empty or desired root is reached.
            return Shell.Current.GoToAsync("///MainPage", animate); // Adjust "MainPage" to your actual root page route
        }
    }
}