namespace WhiteArrow.ReactiveUI.Auth
{
    public static class AuthInputValidator
    {
        public const int MIN_NAME_LENGTH = 2;
        public const int MIN_PASSWORD_LENGTH = 6;



        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.Length >= MIN_NAME_LENGTH;
        }

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && email.Contains("@");
        }

        public static bool IsValidPassword(string password)
        {
            return !string.IsNullOrWhiteSpace(password) && password.Length >= MIN_PASSWORD_LENGTH;
        }

        public static bool IsValidPassword(string password, string confirmPassword)
        {
            return IsValidPassword(password) && password == confirmPassword;
        }
    }
}