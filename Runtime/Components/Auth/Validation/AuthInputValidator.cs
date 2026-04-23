namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public static class AuthInputValidator
    {
        public const int MIN_NAME_LENGTH = 2;
        public const int MIN_PASSWORD_LENGTH = 6;



        public static AuthValidationResult ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new(AuthValidationField.Name, AuthValidationIssue.Empty);

            if (name.Length < MIN_NAME_LENGTH)
                return new(AuthValidationField.Name, AuthValidationIssue.NameTooShort);

            return AuthValidationResult.Valid;
        }

        public static AuthValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new(AuthValidationField.Email, AuthValidationIssue.Empty);

            if (!email.Contains("@"))
                return new(AuthValidationField.Email, AuthValidationIssue.EmailInvalid);

            return AuthValidationResult.Valid;
        }

        public static AuthValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return new(AuthValidationField.Password, AuthValidationIssue.Empty);

            if (password.Length < MIN_PASSWORD_LENGTH)
                return new(AuthValidationField.Password, AuthValidationIssue.PasswordTooShort);

            return AuthValidationResult.Valid;
        }

        public static AuthValidationResult ValidatePasswordConfirmation(string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(confirmPassword))
                return new(AuthValidationField.ConfirmPassword, AuthValidationIssue.Empty);

            if (password != confirmPassword)
                return new(AuthValidationField.ConfirmPassword, AuthValidationIssue.PasswordMismatch);

            return AuthValidationResult.Valid;
        }



        public static string GetErrorMessage(AuthValidationResult result)
        {
            switch (result.Issue)
            {
                case AuthValidationIssue.Empty:
                    return GetEmptyFieldMessage(result.Field);
                case AuthValidationIssue.NameTooShort:
                    return $"Name must be at least {MIN_NAME_LENGTH} characters";
                case AuthValidationIssue.EmailInvalid:
                    return "Email format is invalid";
                case AuthValidationIssue.PasswordTooShort:
                    return $"Password must be at least {MIN_PASSWORD_LENGTH} characters";
                case AuthValidationIssue.PasswordMismatch:
                    return "Passwords do not match";
                default:
                    return "Input is invalid";
            }
        }

        private static string GetEmptyFieldMessage(AuthValidationField field)
        {
            switch (field)
            {
                case AuthValidationField.Name:
                    return "Name is required";
                case AuthValidationField.Email:
                    return "Email is required";
                case AuthValidationField.Password:
                    return "Password is required";
                case AuthValidationField.ConfirmPassword:
                    return "Password confirmation is required";
                default:
                    return "Field is required";
            }
        }
    }
}
