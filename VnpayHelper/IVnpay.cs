using Microsoft.AspNetCore.Http;
using VnpayHelper.Models;

namespace VnpayHelper
{
    public interface IVnpay
    {
        string CreatePaymentUrl(HttpContext context, PaymentRequest model, string uid);
        PaymentResponse PaymentExecute(IQueryCollection collections);
    }
}
