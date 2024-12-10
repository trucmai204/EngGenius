namespace VnpayHelper.Models
{
    public class PaymentRequest
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PaymentDescription { get; set; }
        public double Money { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
