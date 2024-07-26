using BookingTourWeb_WebAPI.Models.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public TaiKhoanController(DVMayBayContext context)
        {
            _context = context;
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
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel request)
        {
            var account = await _context.Taikhoans.Where(f => f.MaTaiKhoan == request.accountId).FirstOrDefaultAsync();

            if (account == null)
            {
                return NotFound();
            }
            if (ToHash(request.oldPassword) != account.MatKhau)
            {
                return BadRequest();
            }
            account.MatKhau = ToHash(request.newPassword);
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(request.accountId))
                {
                    return NotFound("Account not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool TaiKhoanExists(long id)
        {
            return (_context.Taikhoans?.Any(e => e.MaTaiKhoan == id)).GetValueOrDefault();
        }
    }
}
