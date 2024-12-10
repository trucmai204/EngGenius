﻿namespace VnpayHelper.Models
{
    public class PaymentResponse
    {
        public bool IsSuccess { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string Uid { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}
