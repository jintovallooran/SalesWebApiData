namespace SalesManagement.Domain.Models.Order
{
    public class SearchModel:OrderBaseModel
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
