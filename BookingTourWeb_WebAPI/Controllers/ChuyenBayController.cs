﻿using BookingTourWeb_WebAPI.Models.InputModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace BookingTourWeb_WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChuyenBayController : ControllerBase
    {
        private readonly DvmayBayContext _context;

        public ChuyenBayController(DvmayBayContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Chuyenbay>>> GetAllAsync()
        {
            return await _context.Chuyenbays.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<List<Chuyenbay>>> FilterChuyenBayAsync(InputFilterChuyenBay input)
        {
            var data = _context.Chuyenbays.AsQueryable();
            if (input == null)
            {
                return await _context.Chuyenbays.ToListAsync();
            }
            if (!string.IsNullOrEmpty(input.fromPlace))
            {
                data = data.Where(f => f.NoiXuatPhat == input.fromPlace);
            }
            if (!string.IsNullOrEmpty(input.toPlace))
            {
                data = data.Where(f => f.NoiDen == input.toPlace);
            }
            if (!string.IsNullOrEmpty(input.startDate))
            {
                data = data.Where(f => f.NgayXuatPhat >= (DateTime.Parse(input.startDate)));
            }

            return Ok(await data.ToListAsync());
            //var result = await _context.Chuyenbays.Where(x => x.NoiXuatPhat == input.fromPlace && x.NoiDen == input.toPlace && x.NgayXuatPhat >= DateTime.Parse(input.startDate)).ToListAsync();
            //return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<object>> GetByCodeAsync(string code)
        {
            var chuyenBay = await _context.Chuyenbays.Where(x => x.MaChuyenBay == code).Join(_context.Maybays, x => x.MaMayBay, mayBay => mayBay.MaMayBay, (x, mayBay) => new
            {
                chuyenBay = x,
                mayBay = mayBay
            }).FirstOrDefaultAsync();
            if (chuyenBay != null)
            {
                return Ok(chuyenBay);
            }
            return NotFound();
        }

    }
}
