namespace WhiteArrow.ReactiveUI.Auth
{
    public readonly struct AuthCredentials
    {
        public readonly string Email;
        public readonly string Password;



        public AuthCredentials(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}