namespace WhiteArrow.ReactiveUI.Components.Auth
{
    public readonly struct AuthCredentials
    {
        public readonly string Email;
        public readonly string Password;
        public readonly string Name;



        public AuthCredentials(string email, string password, string name = "Guest")
        {
            Email = email;
            Password = password;
            Name = name;
        }
    }
}