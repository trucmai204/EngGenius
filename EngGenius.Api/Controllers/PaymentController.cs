using EngGenius.Database;
using Microsoft.AspNetCore.Mvc;
using VnpayHelper;
using VnpayHelper.Models;

namespace EngGenius.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IVnpay vnPayservice, AppDbContext db) : ControllerBase
    {
        private readonly AppDbContext _db = db;
        private readonly IVnpay _vnpay = vnPayservice;

        [HttpGet("Create")]
        public async Task<ActionResult<string>> Create(int userId, double money)
        {
            var user = await _db.User.FindAsync(userId);
            if (user == null)
            {
                return BadRequest("Người dùng không tồn tại");
            }

            var request = new PaymentRequest
            {
                Money = money,
                PaymentDescription = $"Nâng cấp tài khoản cho người dùng: abc",
            };

            var paymentUrl = _vnpay.CreatePaymentUrl(HttpContext, request, DateTime.Now.ToString());

            return Ok(paymentUrl);
        }
    }
}
