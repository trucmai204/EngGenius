using EngGenius.Api.DTO;
using EngGenius.Database;
using EngGenius.Domains.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Web;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public PaymentController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    private readonly AppDbContext _db;
    public PaymentController(AppDbContext db)
    {
        _db = db;
    }
    [HttpPost("create-payment")]
    public IActionResult CreatePayment([FromBody] PaymentRequestDTO paymentRequest)
    {
        var vnpayConfig = _configuration.GetSection("VNPAY");
        var vnpUrl = vnpayConfig["ApiUrl"];
        var merchantCode = vnpayConfig["MerchantCode"];
        var secretKey = vnpayConfig["SecretKey"];
        var returnUrl = vnpayConfig["ReturnUrl"];
        var notifyUrl = vnpayConfig["NotifyUrl"];

        // Tạo các tham số cần thiết cho yêu cầu thanh toán
        var parameters = new Dictionary<string, string>
        {
            { "vnp_TmnCode", merchantCode },
            { "vnp_Amount", (paymentRequest.Amount * 100).ToString() },
            { "vnp_OrderInfo", paymentRequest.OrderInfo },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_Note", paymentRequest.Note },
            { "vnp_TransactionNo", DateTime.Now.ToString("yyyyMMddHHmmss") },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
        };

        // Tạo chữ ký bảo mật cho yêu cầu
        var hashData = string.Join("&", parameters.OrderBy(p => p.Key).Select(p => $"{p.Key}={p.Value}"));
        var secureHash = HMACSHA512(secretKey, hashData);

        parameters.Add("vnp_SecureHash", secureHash);

        // Gửi yêu cầu thanh toán đến VNPAY
        var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
        var paymentUrl = $"{vnpUrl}?{queryString}";

        return Redirect(paymentUrl);
    }

    private string HMACSHA512(string key, string data)
    {
        using (var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(key)))
        {
            var hashBytes = hmac.ComputeHash(Encoding.ASCII.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
    public async Task<IActionResult> PaymentReturn(string vnp_TransactionNo, string vnp_SecureHash, string vnp_ResponseCode, [FromQuery] int userId)
    {
        var vnpayConfig = _configuration.GetSection("VNPAY");
        var secretKey = vnpayConfig["SecretKey"];

        // Lấy các tham số từ query string và loại bỏ vnp_SecureHash
        var parameters = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString())
                            .Where(x => x.Key != "vnp_SecureHash") // Loại bỏ tham số vnp_SecureHash
                            .OrderBy(p => p.Key)
                            .Select(p => $"{p.Key}={p.Value}")
                            .ToList();

        var hashData = string.Join("&", parameters); // Tạo chuỗi hashData
        var secureHash = HMACSHA512(secretKey, hashData); // Tính toán chữ ký

        // Kiểm tra chữ ký từ VNPAY
        if (secureHash == vnp_SecureHash)
        {
            if (vnp_ResponseCode == "00") // Thanh toán thành công
            {
                var user = await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.PermissionId = EnumPermission.Premium; // Nâng cấp người dùng lên trả phí
                    await _db.SaveChangesAsync(); // Lưu thay đổi
                    return Ok("Thanh toán thành công và nâng cấp người dùng thành công.");
                }
                else
                {
                    return BadRequest("Người dùng không tồn tại.");
                }
            }
            else
            {
                return BadRequest("Thanh toán thất bại.");
            }
        }
        else
        {
            return BadRequest("Chữ ký không hợp lệ.");
        }
    }


}
