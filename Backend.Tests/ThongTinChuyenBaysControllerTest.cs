using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingTourWeb_WebAPI.Controllers;
using BookingTourWeb_WebAPI.Models;
using BookingTourWeb_WebAPI.ViewModels;
using BookingTourWeb_WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Backend.Tests
{
    public class ThongTinChuyenBaysControllerTests
    {
        private readonly ThongTinChuyenBaysController _controller;
        private readonly DVMayBayContext _context;
        private readonly DbContextOptions<DVMayBayContext> _contextOptions;


        public ThongTinChuyenBaysControllerTests()
        {
            // Sử dụng InMemory Database cho kiểm thử
            _contextOptions = new DbContextOptionsBuilder<DVMayBayContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            _context = new DVMayBayContext(_contextOptions);

            // Seed dữ liệu mẫu cho database
            SeedDatabase();

            _controller = new ThongTinChuyenBaysController(_context);
        }

        private void SeedDatabase()
        {
            _context.Maybays.RemoveRange(_context.Maybays);
            _context.Chuyenbays.RemoveRange(_context.Chuyenbays);
            _context.Chitietves.RemoveRange(_context.Chitietves);
            _context.SaveChanges();
            // Thêm dữ liệu mẫu vào database
            _context.Maybays.AddRange(new List<Maybay>
            {
                new Maybay { MaMayBay = "PlaneAB", TenMayBay = "Máy bay Air Bud", SlgheBsn = 10, SlgheEco = 25 },
                new Maybay { MaMayBay = "PlaneMT", TenMayBay = "Máy bay Metro", SlgheBsn = 10, SlgheEco = 30 },
                new Maybay { MaMayBay = "PlaneTH", TenMayBay = "Máy bay The Hills", SlgheBsn = 8, SlgheEco = 40 },
                new Maybay { MaMayBay = "PlaneVJ", TenMayBay = "Máy bay VietJack", SlgheBsn = 12, SlgheEco = 25 },
            });
            _context.SaveChanges();

            _context.Chuyenbays.AddRange(new List<Chuyenbay>
            {
                new Chuyenbay { MaChuyenBay = "070124JPTOUKVJ", NoiXuatPhat = "NHATBAN", NoiDen = "ANH", NgayXuatPhat = new DateTime(2024, 1, 7), MaMayBay = "PlaneVJ" },
                new Chuyenbay { MaChuyenBay = "251223VNTOTHVJ", NoiXuatPhat = "VIETNAM", NoiDen = "MY", NgayXuatPhat = new DateTime(2024, 1, 7), MaMayBay = "PlaneMT" },
            });
            _context.SaveChanges();

            _context.Chitietves.AddRange(new List<Chitietve>
            {
                new Chitietve { MaChuyenBay = "070124JPTOUKVJ", LoaiVe = "BSN", SoLuong = 2, TinhTrang = "Đã đặt", TongGia = 15000, MaVe= 1150 },
                new Chitietve { MaChuyenBay = "251223VNTOTHVJ", LoaiVe = "ECO", SoLuong = 5, TinhTrang = "Đã đặt", TongGia = 15000, MaVe= 1151 },
                // Thêm các chi tiết vé khác nếu cần, ví dụ cho chuyến bay "251223VNTOTHVJ"
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetThongTinChuyenBay_ReturnsOkResult_WhenDataExists()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBay("070124JPTOUKVJ", "", "", "");
;
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<FlightInfo>>(okResult.Value);
            Assert.Single(value);

        }

        [Fact]
        public async Task GetThongTinChuyenBay_ReturnsListOfChuyenBays()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBay(null, null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<FlightInfo>>(okResult.Value);
            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task GetThongTinChuyenBay_FiltersByMaChuyenBay()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBay("0701", null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<FlightInfo>>(okResult.Value);
            Assert.NotEmpty(value);
        }

        [Fact]
        public async Task GetThongTinChuyenBay_ReturnsNotFound_WhenNoFlightsMatch()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBay("61415161", null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<FlightInfo>>(okResult.Value);
            Assert.Empty(value);
        }

        [Fact]
        public async Task GetThongTinChuyenBayByMaChuyen_ReturnsFlightsMatch()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBayByMaChuyenBay("070124JPTOUKVJ");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<ThongTinChuyenBay>>(okResult.Value);
            Assert.Single(value);
        }

        [Fact]
        public async Task GetThongTinChuyenBayByMaChuyen_ReturnsNoFlightsMatch()
        {
            // Act
            var result = await _controller.GetThongTinChuyenBayByMaChuyenBay("070124JPTOUKTH");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsAssignableFrom<IEnumerable<ThongTinChuyenBay>>(okResult.Value);
            Assert.Empty(value);
        }



        [Fact]
        public async Task PostThongTinChuyenBay_ReturnsCreatedAtAction_WithValidData()
        {
            // Arrange
            var newFlight = new ThongTinChuyenBay
            {
                MaChuyenBay = "NEWFLIGHT123",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 1000000,
            };

            // Act
            var result = await _controller.PostThongTinChuyenBay(newFlight);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetThongTinChuyenBay", createdResult.ActionName);
            Assert.Equal(newFlight.MaChuyenBay, createdResult.RouteValues["id"]);

            var returnedFlight = Assert.IsType<ThongTinChuyenBay>(createdResult.Value);
            Assert.Equal(newFlight.MaChuyenBay, returnedFlight.MaChuyenBay);
            // ... Kiểm tra các thuộc tính khác nếu cần

            // Kiểm tra xem chuyến bay đã được thêm vào database
            Assert.True(ThongTinChuyenBayExists(newFlight.MaChuyenBay)); // Bạn cần implement hàm này
        }


        [Fact]
        public async Task PostThongTinChuyenBay_ReturnsBadRequest_WithNullData()
        {
            // Act
            var result = await _controller.PostThongTinChuyenBay(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        [Fact]
        public async Task PostThongTinChuyenBay_ThrowsException_WithInvalidMaMayBay()
        {
            // Arrange
            var newFlight = new ThongTinChuyenBay
            {
                MaChuyenBay = "INVALIDMAYBAY",
                MaMayBay = "INVALID_PLANE",
            };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _controller.PostThongTinChuyenBay(newFlight));
        }

        [Fact]
        public async Task PutThongTinChuyenBay_ReturnsNoContent_WithValidData()
        {
            // Arrange
            var newFlight = new ThongTinChuyenBay
            {
                MaChuyenBay = "NEWFLIGHT123",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 1000000,
            };

            await _controller.PostThongTinChuyenBay(newFlight);

            var updatedFlight = new ThongTinChuyenBay
            {
                MaChuyenBay = "NEWFLIGHT123",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 9000000,
            };


            // Act
            var result = await _controller.PutThongTinChuyenBay(updatedFlight.MaChuyenBay, updatedFlight);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Kiểm tra xem dữ liệu đã được cập nhật trong database
            var updatedChuyenBayInDb = await _context.Chuyenbays.FindAsync(updatedFlight.MaChuyenBay);
            Assert.Equal(updatedFlight.MaMayBay, updatedChuyenBayInDb.MaMayBay);
        }


        [Fact]
        public async Task PutThongTinChuyenBay_ReturnsBadRequest_WithMismatchedIds()
        {
            // Arrange
            var existingFlight = new ThongTinChuyenBay {
                MaChuyenBay = "NEWFLIGHT1",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 9000000,
            };
            var updatedFlight = new ThongTinChuyenBay {
                MaChuyenBay = "NEWFLIGHT2",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 9000000,
            };

            // Act
            var result = await _controller.PutThongTinChuyenBay(existingFlight.MaChuyenBay, updatedFlight);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }



        [Fact]
        public async Task PutThongTinChuyenBay_ReturnsNotFound_WithNonExistingFlight()
        {
            // Arrange
            var nonExistingFlight = new ThongTinChuyenBay {
                MaChuyenBay = "NEWFLIGHT1",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 9000000,
            };


            // Act
            var result = await _controller.PutThongTinChuyenBay(nonExistingFlight.MaChuyenBay, nonExistingFlight);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);


        }

        [Fact]
        public async Task DeleteThongTinChuyenBay_ReturnsNoContent_WithExistingFlight()
        {
            // Arrange
            var existingFlight = new ThongTinChuyenBay {
                MaChuyenBay = "NEWFLIGHT1",
                MaMayBay = "PlaneAB",
                GioBay = new TimeSpan(10, 0, 0),
                NoiXuatPhat = "ANH",
                NoiDen = "VIETNAM",
                NgayXuatPhat = new DateTime(2024, 12, 25),
                DonGia = 9000000,
            };
            await _controller.PostThongTinChuyenBay(existingFlight);

            // Act
            var result = await _controller.DeleteThongTinChuyenBay(existingFlight.MaChuyenBay);

            // Assert
            Assert.IsType<NoContentResult>(result);

            // Kiểm tra xem chuyến bay đã bị xóa khỏi database
            Assert.False(ThongTinChuyenBayExists(existingFlight.MaChuyenBay));
        }

        [Fact]
        public async Task DeleteThongTinChuyenBay_ReturnsNotFound_WithNonExistingFlight()
        {
            // Arrange
            var nonExistingFlight = "NONEXISTING101"; // Mã chuyến bay không tồn tại

            // Act
            var result = await _controller.DeleteThongTinChuyenBay(nonExistingFlight);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Chuyenbay not found.", notFoundResult.Value);
        }

        private bool ThongTinChuyenBayExists(string maChuyenBay)
        {
            return _context.Chuyenbays.Any(e => e.MaChuyenBay == maChuyenBay);
        }
    }
}
