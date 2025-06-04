using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using QuizAppPG.Views.Auth;
using Microsoft.AspNetCore.SignalR.Client;

namespace QuizAppPG
{
    public partial class AppShell : Shell
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;

        public IRelayCommand LogoutCommand { get; }

        public AppShell(ISecureStorageService secureStorageService, INavigationService navigationService, IDialogService dialogService)
        {
            InitializeComponent();

            _secureStorageService = secureStorageService;
            _navigationService = navigationService;
            _dialogService = dialogService;

            LogoutCommand = new RelayCommand(async () => await LogoutAsync());
            var currentTheme = Application.Current!.UserAppTheme;
            if (ThemeSegmentedControl != null)
            {
                ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
            }

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(Views.MainPage), typeof(Views.MainPage));

            // User Pages
            Routing.RegisterRoute(nameof(Views.User.ProfilePage), typeof(Views.User.ProfilePage));
            Routing.RegisterRoute(nameof(Views.User.EditProfilePage), typeof(Views.User.EditProfilePage));
            Routing.RegisterRoute(nameof(Views.User.ChangePasswordPage), typeof(Views.User.ChangePasswordPage));
            Routing.RegisterRoute(nameof(Views.User.UserSearchPage), typeof(Views.User.UserSearchPage));

            // Friend Pages
            Routing.RegisterRoute(nameof(Views.Friend.FriendsPage), typeof(Views.Friend.FriendsPage));
            Routing.RegisterRoute(nameof(Views.Friend.AddFriendPage), typeof(Views.Friend.AddFriendPage));

            // Game Pages
            Routing.RegisterRoute(nameof(Views.Game.CreateGamePage), typeof(Views.Game.CreateGamePage));
            Routing.RegisterRoute(nameof(Views.Game.GameLobbyPage), typeof(Views.Game.GameLobbyPage));
            Routing.RegisterRoute(nameof(Views.Game.MultiplayerGamePage), typeof(Views.Game.MultiplayerGamePage));

            // Quiz Pages
            Routing.RegisterRoute(nameof(Views.Quiz.QuizCategoriesPage), typeof(Views.Quiz.QuizCategoriesPage));
            Routing.RegisterRoute(nameof(Views.Quiz.SoloQuizPage), typeof(Views.Quiz.SoloQuizPage));

            // Leaderboard Pages
            Routing.RegisterRoute(nameof(Views.Leaderboard.LeaderboardPage), typeof(Views.Leaderboard.LeaderboardPage));
        }

        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"),
                TextColor = Colors.White,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(0),
                Font = Microsoft.Maui.Font.SystemFontOfSize(18),
                ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14)
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);
            await snackbar.Show(cancellationTokenSource.Token);
        }

        public static async Task DisplayToastAsync(string message)
        {
            if (OperatingSystem.IsWindows())
                return;
            var toast = Toast.Make(message, textSize: 18);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await toast.Show(cts.Token);
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
        }

        private async Task LogoutAsync()
        {
            bool confirm = await _dialogService.ShowConfirmAsync("Logout", "Are you sure you want to log out?");
            if (confirm)
            {
                _secureStorageService.ClearAll();
                var gameHubClient = ServiceHelper.GetService<IGameHubClient>();
                if (gameHubClient != null && gameHubClient.State != HubConnectionState.Disconnected)
                {
                    await gameHubClient.DisconnectAsync();
                }

                await _navigationService.GoToAsync($"//{nameof(LoginPage)}", animate: false);
            }
        }
    }
}