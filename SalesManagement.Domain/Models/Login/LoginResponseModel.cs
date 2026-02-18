namespace SalesManagement.Domain.Models.Login
{
    public class LoginResponseModel :NotifyModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
