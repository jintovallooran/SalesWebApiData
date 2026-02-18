namespace SalesManagement.Domain.Models.Login
{
    public class UserClaimModel
    {
        public long? UserId { get; set; }
        public int? MsgTypeId { get; set; }
        public short? CompanyCode { get; set; }
        public string? Language { get; set; } = string.Empty;
        public string? Username { get; set; } = string.Empty;

        public string? Msg { get; set; } = string.Empty;
        public string? AppType { get; set; } = string.Empty;
    }
}
