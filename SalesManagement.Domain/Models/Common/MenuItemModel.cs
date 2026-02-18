namespace SalesManagement.Domain.Models.Common
{
    public class MenuItemModel
    {
        public string? Label { get; set; }
        public string? Icon { get; set; }
        public string? RouterLink { get; set; }
        public List<MenuItemModel> Items { get; set; } = new List<MenuItemModel>();
    }
}
