namespace SalesManagement.Domain.Constants
{
    public static class DBConstants
    {
        public static class Parameters
        {
            public static string UserName = "@as_user_name";
            public static string Password = "@as_pwd";
            public static string UserId = "@ai_user_id";
            public static string RoleId = "@ai_role_id";
            public static string OrderId = "@ai_order_id";
            public static string VendorId = "@ai_vendor_id";
            public static string TableItems = "@t_items";
            public static string OpsMode = "@as_ops_mode";
            public static string FromDate = "@ad_from_date";
            public static string ToDate = "@ad_to_date";

        }
        public static class OutParameters
        {
            public static string Msg = "@p_msg";
            public static string MsgType = "@p_msg_type";
            public static string RetId = "@p_ret_id";
        }
        public static class Procedures
        {
            public static string ValidateLogin = "PKG_AD$validate_login";
            public static string GetUserRole = "PKG_AD$get_user_role";
            public static string SaveOrderDetails = "PKG_ORD$save_order_details";
            public static string GetItemList = "PKG_AD$get_item_list";
            public static string GetOrderNo = "PKG_ORD$get_order_no";
            public static string GetOrderList = "PKG_ORD$get_order_list";
            public static string GetOrderItemList = "PKG_AD$get_order_item_list";
            public static string GetVendorList = "PKG_AD$get_vendor_list";
            public static string GetUserList = "PKG_AD$get_user_list";
        }
    }
}
