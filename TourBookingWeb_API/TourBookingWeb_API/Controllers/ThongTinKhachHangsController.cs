﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TourBookingWeb_API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourBookingWeb_API.Controllers;
using TourBookingWeb_API.Models;

namespace TourBookingWeb_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinKhachhangsController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public ThongTinKhachhangsController(DVMayBayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThongTinKhachHang>>> GetThongTinKhachHang()
        {
            //return await _context.ThongTinKhachHang.ToListAsync();
            var ThongTinKhachHang = (from khachhang in _context.Khachhangs
                                     join Ve in _context.Ves
                                     on khachhang.MaKh equals Ve.MaKh
                                     join Chitietve in _context.Chitietves
                                     on Ve.MaVe equals Chitietve.MaVe
                                     join chuyenbay in _context.Chuyenbays
                                     on Chitietve.MaChuyenBay equals chuyenbay.MaChuyenBay
                                     select new ThongTinKhachHang()
                                     {
                                         Makhachhang = khachhang.MaKh,
                                         HoTenKh = khachhang.TenKh,
                                         Phai = khachhang.Phai,
                                         GmailKh = khachhang.GmailKh,
                                         MaChuyenBay=Chitietve.MaChuyenBay,
                                         MaVe=Ve.MaVe,
                                         Sdt=khachhang.Sdt
                                     })
            .ToListAsync();
            //return Ok(ThongTinKhachHang);
            return await ThongTinKhachHang;
        }
        //===========================================================================

        [HttpGet("{makhachhang}")]
        public async Task<ActionResult<IEnumerable<ThongTinKhachHang>>> GetThongTinKhachHangByMakhachhang(long makhachhang)
        {
            var ThongTinKhachHang = await (from khachhang in _context.Khachhangs
                                           join Ve in _context.Ves
                                           on khachhang.MaKh equals Ve.MaKh
                                           join Chitietve in _context.Chitietves
                                           on Ve.MaVe equals Chitietve.MaVe
                                           join chuyenbay in _context.Chuyenbays
                                           on Chitietve.MaChuyenBay equals chuyenbay.MaChuyenBay
                                           where khachhang.MaKh.Equals(makhachhang)
                                           select new ThongTinKhachHang()
                                           {
                                               Makhachhang = khachhang.MaKh,
                                               HoTenKh = khachhang.TenKh,
                                               Phai = khachhang.Phai,
                                               GmailKh = khachhang.GmailKh,
                                               MaChuyenBay = Chitietve.MaChuyenBay,
                                               MaVe = Ve.MaVe,
                                           })
            .ToListAsync();

            if (ThongTinKhachHang == null)
            {
                return NotFound("ThongTinKhachHang not found.");
            }

            return ThongTinKhachHang;
        }


        //===========================================================================

        [HttpPost]
        public async Task<ActionResult<ThongTinKhachHang>> PostThongTinKhachHang(ThongTinKhachHang ThongTinKhachHang)
        {
            if (ThongTinKhachHang == null)
            {
                return BadRequest("Invalid data.");
            }

            // Chuyển đổi dữ liệu từ ThongTinKhachHang sang khachhang
            var khachhang = new Khachhang
            {
                MaKh = ThongTinKhachHang.Makhachhang,
                TenKh = ThongTinKhachHang.HoTenKh,
                Phai = ThongTinKhachHang.Phai,
                GmailKh = ThongTinKhachHang.GmailKh,
                MaTaiKhoan=ThongTinKhachHang.Makhachhang,
                Sdt=ThongTinKhachHang.Sdt,
            };

            _context.Khachhangs.Add(khachhang);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThongTinKhachHangExists(khachhang.MaKh))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetThongTinKhachHang", new { id = ThongTinKhachHang.Makhachhang }, ThongTinKhachHang);
        }



        //============================================================================

        [HttpPut("{makhachhang}")]
        public async Task<IActionResult> PutThongTinKhachHang(long makhachhang, ThongTinKhachHang ThongTinKhachHang)
        {
            if (makhachhang != ThongTinKhachHang.Makhachhang)
            {
                return BadRequest("Mismatched IDs.");
            }

            var khachhang = await _context.Khachhangs.FindAsync(makhachhang);

            if (khachhang == null)
            {
                return NotFound("khachhang not found.");
            }

            // Cập nhật thông tin khachhang
            khachhang.MaKh = ThongTinKhachHang.Makhachhang;
            khachhang.TenKh = ThongTinKhachHang.HoTenKh;
            khachhang.Phai = ThongTinKhachHang.Phai;
            khachhang.GmailKh = ThongTinKhachHang.GmailKh;
            khachhang.Sdt = ThongTinKhachHang.Sdt;
            khachhang.MaTaiKhoan = ThongTinKhachHang.MaTaiKhoan;

            _context.Entry(khachhang).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThongTinKhachHangExists(makhachhang))
                {
                    return NotFound("khachhang not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //============================================================================

        [HttpDelete("{makhachhang}")]
        public async Task<IActionResult> DeleteThongTinKhachHang(string makhachhang)
        {
            if (_context.ThongTinKhachHang == null)
            {
                return NotFound();
            }

            var khachhang = await _context.Khachhangs.FindAsync(makhachhang);

            if (khachhang == null)
            {
                return NotFound("khachhang not found.");
            }

            _context.Khachhangs.Remove(khachhang);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThongTinKhachHangExists(long id)
        {
            return (_context.ThongTinKhachHang?.Any(e => e.Makhachhang == id)).GetValueOrDefault();
        }
    }
}
