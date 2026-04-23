namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public readonly struct AuthValidationResult
    {
        public readonly AuthValidationField Field;
        public readonly AuthValidationIssue Issue;


        public bool IsValid => Issue == AuthValidationIssue.None;



        public static AuthValidationResult Valid { get; } = new AuthValidationResult(AuthValidationField.None, AuthValidationIssue.None);



        public AuthValidationResult(AuthValidationField field, AuthValidationIssue issue)
        {
            Field = field;
            Issue = issue;
        }
    }
}
