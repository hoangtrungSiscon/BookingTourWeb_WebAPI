﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTourWeb_WebAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingTourWeb_WebAPI.Controllers;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongTinChuyenBaysController : ControllerBase
    {
        private readonly DVMayBayContext _context;

        public ThongTinChuyenBaysController(DVMayBayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightInfo>>> GetThongTinChuyenBay(string? MaChuyenBay, string? NoiXuatPhat, string? NoiDen, string? NgayXuatPhat)
        {
            var query = _context.Chuyenbays.Include(f => f.MaMayBayNavigation).AsQueryable();

            if (MaChuyenBay != "" && !string.IsNullOrEmpty(MaChuyenBay))
            {
                query = query.Where(f => f.MaChuyenBay.Contains(MaChuyenBay));
            }
            if (NoiXuatPhat != "" && !string.IsNullOrEmpty(NoiXuatPhat))
            {
                query = query.Where(f => f.NoiXuatPhat == NoiXuatPhat);
            }
            if (NoiDen != "" && !string.IsNullOrEmpty(NoiDen))
            {
                query = query.Where(f => f.NoiDen == NoiDen);
            }
            if (NgayXuatPhat != "" && !string.IsNullOrEmpty(NgayXuatPhat))
            {
                query = query.Where(f => (f.NgayXuatPhat.Year + "-" + f.NgayXuatPhat.Month + "-" + f.NgayXuatPhat.Day) == NgayXuatPhat);
            }

            var thongtinchuyenbay = await query.Select(f => new FlightInfo
            {
                MaChuyenBay = f.MaChuyenBay,
                MaMayBay = f.MaMayBay,
                TenMayBay = f.MaMayBayNavigation.TenMayBay,
                NoiXuatPhat = f.NoiXuatPhat,
                NoiDen = f.NoiDen,
                NgayXuatPhat = f.NgayXuatPhat,
                GioBay = f.GioBay,
                DonGia = f.DonGia,

                SoLuongVeBsn = _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong).ToString() + '/' + f.MaMayBayNavigation.SlgheBsn.ToString(),
                SoLuongVeEco = _context.Chitietves.Where(c => c.MaChuyenBay == f.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong).ToString() + '/' + f.MaMayBayNavigation.SlgheEco.ToString(),
            }).ToListAsync();

            if (thongtinchuyenbay == null)
            {
                return NotFound("ThongTinChuyenBay not found.");
            }
            //return Ok(thongtinchuyenbay);
            return Ok(thongtinchuyenbay);
        }
        //===========================================================================

        [HttpGet("{maChuyenBay}")]
        public async Task<ActionResult<IEnumerable<ThongTinChuyenBay>>> GetThongTinChuyenBayByMaChuyenBay(string maChuyenBay)
        {
            var thongtinchuyenbay = await (from chuyenbay in _context.Chuyenbays
                                           join maybay in _context.Maybays
                                           on chuyenbay.MaMayBay equals maybay.MaMayBay
                                           where chuyenbay.MaChuyenBay.Contains(maChuyenBay)
                                           select new ThongTinChuyenBay()
                                           {
                                               MaChuyenBay = chuyenbay.MaChuyenBay,
                                               MaMayBay = chuyenbay.MaMayBay,
                                               TenMayBay = maybay.TenMayBay,
                                               NoiXuatPhat = chuyenbay.NoiXuatPhat,
                                               NoiDen = chuyenbay.NoiDen,
                                               NgayXuatPhat = chuyenbay.NgayXuatPhat,
                                               GioBay = chuyenbay.GioBay,
                                               SoLuongVeBsn = _context.Chitietves.Where(c => c.MaChuyenBay == chuyenbay.MaChuyenBay && c.LoaiVe == "BSN" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
                                               SoLuongVeEco = _context.Chitietves.Where(c => c.MaChuyenBay == chuyenbay.MaChuyenBay && c.LoaiVe == "ECO" && c.TinhTrang != "Đã hủy").Sum(b => b.SoLuong),
                                               DonGia = chuyenbay.DonGia,
                                           })
            .ToListAsync();
            if (thongtinchuyenbay == null)
            {
                return NotFound("ThongTinChuyenBay not found.");
            }

            return Ok(thongtinchuyenbay);
        }


        //===========================================================================

        [HttpPost]
        public async Task<ActionResult<ThongTinChuyenBay>> PostThongTinChuyenBay(ThongTinChuyenBay thongTinChuyenBay)
        {
            if (thongTinChuyenBay == null)
            {
                return BadRequest("Invalid data.");
            }

            // Chuyển đổi dữ liệu từ ThongTinChuyenBay sang Chuyenbay
            var chuyenbay = new Chuyenbay
            {
                MaChuyenBay = thongTinChuyenBay.MaChuyenBay,
                MaMayBay = thongTinChuyenBay.MaMayBay,
                GioBay = thongTinChuyenBay.GioBay,
                NoiXuatPhat = thongTinChuyenBay.NoiXuatPhat,
                NoiDen = thongTinChuyenBay.NoiDen,
                NgayXuatPhat = thongTinChuyenBay.NgayXuatPhat,
                DonGia = thongTinChuyenBay.DonGia,
                SoLuongVeBsn = thongTinChuyenBay.SoLuongVeBsn,
                SoLuongVeEco = thongTinChuyenBay.SoLuongVeEco,
            };

            _context.Chuyenbays.Add(chuyenbay);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ThongTinChuyenBayExists(chuyenbay.MaChuyenBay))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetThongTinChuyenBay", new { id = thongTinChuyenBay.MaChuyenBay }, thongTinChuyenBay);
        }



        //============================================================================

        [HttpPut("{maChuyenBay}")]
        public async Task<IActionResult> PutThongTinChuyenBay(string maChuyenBay, ThongTinChuyenBay thongTinChuyenBay)
        {
            if (maChuyenBay != thongTinChuyenBay.MaChuyenBay)
            {
                return BadRequest("Mismatched IDs.");
            }

            var chuyenbay = await _context.Chuyenbays.FindAsync(maChuyenBay);

            if (chuyenbay == null)
            {
                return NotFound("Chuyenbay not found.");
            }

            // Cập nhật thông tin Chuyenbay
            chuyenbay.MaMayBay = thongTinChuyenBay.MaMayBay;
            chuyenbay.GioBay = thongTinChuyenBay.GioBay;
            chuyenbay.NoiXuatPhat = thongTinChuyenBay.NoiXuatPhat;
            chuyenbay.NoiDen = thongTinChuyenBay.NoiDen;
            chuyenbay.NgayXuatPhat = thongTinChuyenBay.NgayXuatPhat;
            chuyenbay.DonGia = thongTinChuyenBay.DonGia;
            chuyenbay.SoLuongVeBsn = thongTinChuyenBay.SoLuongVeBsn;
            chuyenbay.SoLuongVeEco = thongTinChuyenBay.SoLuongVeEco;

            _context.Entry(chuyenbay).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThongTinChuyenBayExists(maChuyenBay))
                {
                    return NotFound("Chuyenbay not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //============================================================================

        [HttpDelete("{maChuyenBay}")]
        public async Task<IActionResult> DeleteThongTinChuyenBay(string maChuyenBay)
        {
            if (_context.Chuyenbays == null)
            {
                return NotFound();
            }

            var chuyenbay = await _context.Chuyenbays.FindAsync(maChuyenBay);

            if (chuyenbay == null)
            {
                return NotFound("Chuyenbay not found.");
            }

            _context.Chuyenbays.Remove(chuyenbay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThongTinChuyenBayExists(string id)
        {
            return (_context.Chuyenbays?.Any(e => e.MaChuyenBay == id)).GetValueOrDefault();
        }
    }
}
