using System; // For TimeSpan
using System.Text.RegularExpressions;

namespace QuizAppPG.Utilities.Validators // Changed QuizAppFrontend to QuizAppPG
{
    public static class ValidationRules
    {
        // Basic Email Validation
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Use a simple regex pattern for email validation
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        // Example: Password strength validation
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            // At least one uppercase letter
            // At least one lowercase letter
            // At least one digit
            // At least one special character
            // You can customize these rules.
            var hasUpper = new Regex(@"[A-Z]+");
            var hasLower = new Regex(@"[a-z]+");
            var hasDigit = new Regex(@"[0-9]+");
            // var hasSpecialChar = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"); // Example for special chars

            return hasUpper.IsMatch(password) && hasLower.IsMatch(password) && hasDigit.IsMatch(password);
        }

        // Example: Username validation (e.g., no spaces, min/max length)
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
                return false;

            // No spaces allowed in username
            if (username.Contains(" "))
                return false;

            // You can add more complex rules like disallowed characters
            return true;
        }
    }
}