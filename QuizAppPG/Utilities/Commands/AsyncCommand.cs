using System.Windows.Input; // For ICommand

namespace QuizAppPG.Utilities.Commands // Changed QuizAppFrontend to QuizAppPG
{
    // This is a basic implementation of an async command.
    // In most cases, if you use CommunityToolkit.Mvvm,
    // [RelayCommand] on an async Task method will suffice.
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute; // Made nullable
        private bool _isExecuting;

        // Marked as nullable to match ICommand's definition
        public event EventHandler? CanExecuteChanged;

        public AsyncCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) // Made parameter nullable
        {
            return !_isExecuting && (_canExecute?.Invoke() ?? true);
        }

        public async void Execute(object? parameter) // Made parameter nullable
        {
            if (CanExecute(parameter))
            {
                try
                {
                    _isExecuting = true;
                    RaiseCanExecuteChanged();
                    await _execute();
                }
                finally
                {
                    _isExecuting = false;
                    RaiseCanExecuteChanged();
                }
            }
        }

        public void RaiseCanExecuteChanged()
        {
            MainThread.BeginInvokeOnMainThread(() => CanExecuteChanged?.Invoke(this, EventArgs.Empty));
        }
    }
}