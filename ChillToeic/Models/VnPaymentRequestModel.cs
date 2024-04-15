namespace ChillToeic.Models
{
    public class VnPaymentRequestModel
    {
        public int OrderId { get; set; }
        public string OrderInfo { get; set; } // Combines ProductName and Description
        public double Amount { get; set; } // TotalAmount after discount, if any
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; } // If needed for the payment gateway
        public int CourseId { get; set; } // If needed for the payment gateway
    }
}
