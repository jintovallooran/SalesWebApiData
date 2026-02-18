namespace SalesManagement.Domain.Models.Order
{
    public class ItemModel: OrderItemsModel
    {
        public string? ItemCode { get; set; }
        public string? ItemName { get; set; }
        public string? Uom { get; set; }

    }
}
