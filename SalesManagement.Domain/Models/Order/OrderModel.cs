namespace SalesManagement.Domain.Models.Order
{
    public class OrderModel : OrderBaseModel
    {
        public long? OrderId { get; set; }
        public long? VendorId { get; set; }
        public string? VendorName { get; set; }
        public string? VendorCode { get; set; }
        public DateTime? OrderDate { get; set; }
        public List<OrderItemsModel> Items { get; set; } = new();
    }

    public class OrderBaseModel : BaseModel
    {
        public string? OrderNo { get; set; }
    }
}
