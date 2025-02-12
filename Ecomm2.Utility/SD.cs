namespace Ecomm2.Utility
{
    public class SD
    {
        // SP Cover Types
        /*
        public const string SP_GetCoverTypes = "GetCoverTypes";
        public const string SP_GetCoverType = "GetCoverType";
        public const string SP_CreateCoverType = "CreateCoverType";
        public const string SP_UpdateCoverType = "UpdateCoverType";
        public const string SP_DeleteCoverType = "DeleteCoverType";
        */
        // For Roles
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee User";
        public const string Role_Company = "Company User";
        public const string Role_Individual = "Individual User";
        //Order Status
        public const string OrderStatusPending = "Pending";
        public const string OrderStatusApproved = "Approved";
        public const string OrderStatusInProgress = "Processing";
        public const string OrderStatusShipped = "Shipped";
        public const string OrderStatusCancelled = "Cancelled";
        public const string OrderStatusRefunded = "Refunded";
        //payment status
        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayPayment = "PaymentStatusDelay";
        public const string PaymentStatusRejected = "Rejected";
        // session variable
        public const string Ss_CartSessionCount = "Cart Count Session";
        // For price
        public static double GetPriceBasedonQunatity(double quantity, double price, double price50, double price100)
        {
            if (quantity < 50)
                return price;
            else if (quantity < 100)
                return price50;
            else
                return price100;
        }
    }
}
