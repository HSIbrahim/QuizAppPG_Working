using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;
using QuizAppPG.ViewModels.Auth;
using QuizAppPG.ViewModels.Game;
using QuizAppPG.ViewModels.User;
using QuizAppPG.ViewModels.Friend;
using QuizAppPG.ViewModels.Quiz;
using QuizAppPG.ViewModels.Leaderboard;

namespace QuizAppPG
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure => configure.AddDebug());
#endif

            // Register Services
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();

            // API Services
            builder.Services.AddSingleton<IAuthApiService, AuthApiService>();
            builder.Services.AddSingleton<IFriendApiService, FriendApiService>();
            builder.Services.AddSingleton<IGameApiService, GameApiService>();
            builder.Services.AddSingleton<ILeaderboardApiService, LeaderboardApiService>();
            builder.Services.AddSingleton<IQuizApiService, QuizApiService>();
            builder.Services.AddSingleton<IUserApiService, UserApiService>();

            // Realtime Service
            builder.Services.AddSingleton<IGameHubClient, GameHubClient>();

            // Register ViewModels
            builder.Services.AddTransient<MainViewModel>();
            IServiceCollection serviceCollection = builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<EditProfileViewModel>();
            builder.Services.AddTransient<ChangePasswordViewModel>();
            builder.Services.AddTransient<UserSearchViewModel>();
            builder.Services.AddTransient<FriendsViewModel>();
            builder.Services.AddTransient<AddFriendViewModel>();
            builder.Services.AddTransient<CreateGameViewModel>();
            builder.Services.AddTransient<GameLobbyViewModel>();
            builder.Services.AddTransient<MultiplayerGameViewModel>();
            builder.Services.AddTransient<QuizCategoriesViewModel>();
            builder.Services.AddTransient<SoloQuizViewModel>();
            builder.Services.AddTransient<LeaderboardViewModel>();

            // Register Pages
            builder.Services.AddTransient<Views.Auth.LoginPage>();
            builder.Services.AddTransient<Views.Auth.RegisterPage>();
            builder.Services.AddTransient<Views.MainPage>();
            builder.Services.AddTransient<Views.User.ProfilePage>();
            builder.Services.AddTransient<Views.User.EditProfilePage>();
            builder.Services.AddTransient<Views.User.ChangePasswordPage>();
            builder.Services.AddTransient<Views.User.UserSearchPage>();
            builder.Services.AddTransient<Views.Friend.FriendsPage>();
            builder.Services.AddTransient<Views.Friend.AddFriendPage>();
            builder.Services.AddTransient<Views.Game.CreateGamePage>();
            builder.Services.AddTransient<Views.Game.GameLobbyPage>();
            builder.Services.AddTransient<Views.Game.MultiplayerGamePage>();
            builder.Services.AddTransient<Views.Quiz.QuizCategoriesPage>();
            builder.Services.AddTransient<Views.Quiz.SoloQuizPage>();
            builder.Services.AddTransient<Views.Leaderboard.LeaderboardPage>();

            builder.Services.AddSingleton<AppShell>();

            var app = builder.Build();
            ServiceHelper.Services = app.Services;
            return app;
        }
    }
}