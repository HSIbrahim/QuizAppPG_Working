namespace QuizAppPG.Utilities
{
    public static class ServiceHelper
    {
        public static IServiceProvider? Services { get; internal set; }

        public static TService GetService<TService>()
            where TService : class
        {
            if (Services == null)
            {
                throw new InvalidOperationException("Service collection is not initialized. Call UseMauiApp before attempting to get services.");
            }
            var service = Services.GetService<TService>();
            if (service == null)
            {
                throw new InvalidOperationException($"Service of type {typeof(TService).FullName} not found. Please ensure it's registered in MauiProgram.cs.");
            }
            return service;
        }
    }
}