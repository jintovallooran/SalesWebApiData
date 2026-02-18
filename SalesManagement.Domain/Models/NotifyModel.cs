namespace SalesManagement.Domain.Models
{
    public enum MessageType
    {
        error = 0,
        warn = 1,
        success = 2
    }
    public class NotifyModel
    {
        public NotifyModel(string msg, Int16 msgType, string userId = "", string? retValue = "", string? retMsg = "")
        {
            Message = msg;
            MessageType = ((MessageType)msgType).ToString();
            UserId = userId;
            retValue = retValue ?? "";
            retMsg = retMsg ?? "";
        }
        public NotifyModel()
        {
        }
        public string? Message { get; set; }
        public string? MessageType { get; set; }
        public string? UserId { get; set; }
        public string? retMsg { get; set; } 
    }
}
