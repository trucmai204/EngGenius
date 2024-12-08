namespace EngGenius.Api.DTO
{
    public class PaymentRequestDTO
    {
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
        public string Note { get; set; }
    }
}
