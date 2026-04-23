namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public enum AuthValidationIssue
    {
        None = 0,
        Empty,
        NameTooShort,
        EmailInvalid,
        PasswordTooShort,
        PasswordMismatch
    }
}
