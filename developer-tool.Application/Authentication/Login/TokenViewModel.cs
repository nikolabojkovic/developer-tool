namespace Application.Authentication.Commands
{
    public class TokenViewModel
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}