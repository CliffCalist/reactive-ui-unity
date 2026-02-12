namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public readonly struct AuthOperationResult
    {
        public readonly bool IsSuccess;
        public readonly string DisplayErrorMessage;

        private AuthOperationResult(bool isSuccess, string displayErrorMessage = null)
        {
            IsSuccess = isSuccess;
            DisplayErrorMessage = displayErrorMessage;
        }



        public static AuthOperationResult Ok() => new(true);
        public static AuthOperationResult Fail(string displayError) => new(false, displayError);
    }
}