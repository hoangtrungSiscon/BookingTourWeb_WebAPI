using BookingTourWeb_WebAPI.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly DvmayBayContext _context;
        private readonly IConfiguration _config;

        public AuthController(DvmayBayContext context, IConfiguration config)
        {
            this._context = context;
            this._config = config;
        }
        private string ToHash(string s)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        [HttpPost]
        public async Task<ActionResult> SendOtpAsync(InputForgetPassword request)
        {
            var checkEmail = await _context.Khachhangs.Where(x => x.GmailKh == request.email).FirstOrDefaultAsync();
            if (checkEmail == null) { return Ok(false); }
            var mail = "flightdotservice@gmail.com";
            var pass = "klqg vnlz udob ugkm";
            var message = new MailMessage();
            message.From = new MailAddress(mail);
            message.To.Add(new MailAddress(request.email));
            message.Subject = "Reset password";
            message.Body = "<html><body> Mã OTP của bạn là: " + request.message + "</body></html>";
            message.IsBodyHtml = true;
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pass),
                EnableSsl = true
            };

            client.Send(message);
            return Ok(request.message);
        }

        [HttpPost]
        public async Task<ActionResult> ChangePasswordAsync(InputChangePassword request)
        {
            var kh = await this._context.Khachhangs.Where(x => x.GmailKh == request.email).FirstOrDefaultAsync();
            var tk = await this._context.Taikhoans.Where(x => x.MaTaiKhoan == kh.MaTaiKhoan).FirstOrDefaultAsync();
            //tk.MatKhau = "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
            
            
            Random random = new Random();

            // Tạo số nguyên ngẫu nhiên với 6 chữ số
            int randomPassword = random.Next(100000, 999999);
            tk.MatKhau = ToHash(randomPassword.ToString());


            this._context.Update(tk);
            await this._context.SaveChangesAsync();
            var mail = "flightdotservice@gmail.com";
            var pass = "klqg vnlz udob ugkm";
            var message = new MailMessage();
            message.From = new MailAddress(mail);
            message.To.Add(new MailAddress(request.email));
            message.Subject = "Reset password";
            message.Body = "<html><body> Mật khẩu mới của bạn là: " + randomPassword + "</body></html>";
            message.IsBodyHtml = true;
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mail, pass),
                EnableSsl = true
            };

            client.Send(message);
            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult> LoginAsync(InputLogin request)
        {
            var checkTK = await _context.Taikhoans.Where(x => x.TaiKhoan1 == request.TaiKhoan1).FirstOrDefaultAsync();
            if (checkTK != null)
            {
                var checkMK = checkTK.MatKhau == ToHash(request.MatKhau);
                if (checkMK == true)
                {
                    var token = checkTK.MaTaiKhoan.ToString() + ' ' + GenerateToken(checkTK)+ ' ' + checkTK.VaiTro.ToString();
                    return Ok(token);
                }
            }
            return Ok(false);
        }

        private string GenerateToken(Taikhoan user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.TaiKhoan1),
                new Claim(ClaimTypes.Role ,user.VaiTro == 0 ? "admin" : "khachHang")
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [HttpPost]
        public async Task<ActionResult<InputRegister>> RegisterAsync(InputRegister request)
        {
            var taiKhoan = new Taikhoan()
            {
                MaTaiKhoan = 0,
                TaiKhoan1 = request.TaiKhoan1,
                MatKhau = ToHash(request.MatKhau),
                VaiTro = 1,
            };
            _context.Taikhoans.Add(taiKhoan);
            if (!TaiKhoanExists(taiKhoan.MaTaiKhoan))
            {
                return Conflict();
            }
            await _context.SaveChangesAsync();
            var khachHang = new Khachhang()
            {
                MaKh = 0,
                MaTaiKhoan = taiKhoan.MaTaiKhoan,
                TenKh = request.TenKH,
                Sdt = request.Sdt,
                GmailKh = request.GamilKH,
                Phai=request.Phai,
            };
            await _context.SaveChangesAsync();
            _context.Khachhangs.Add(khachHang);
            if (!KhachHangExists(khachHang.MaKh))
            {
                return Conflict();
            }
            return request;
        }
        private bool TaiKhoanExists(long id)
        {
            return (_context.Taikhoans?.Any(e => e.MaTaiKhoan == id)).GetValueOrDefault();
        }
        private bool KhachHangExists(long id)
        {
            return (_context.Khachhangs?.Any(e => e.MaKh == id)).GetValueOrDefault();
        }
    }
}