using System.Text.RegularExpressions;

namespace QuizAppPG.Utilities.Validators
{
    public static class ValidationRules
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;
            var hasUpper = new Regex(@"[A-Z]+");
            var hasLower = new Regex(@"[a-z]+");
            var hasDigit = new Regex(@"[0-9]+");

            return hasUpper.IsMatch(password) && hasLower.IsMatch(password) && hasDigit.IsMatch(password);
        }
        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
                return false;
            if (username.Contains(" "))
                return false;
            return true;
        }
    }
}