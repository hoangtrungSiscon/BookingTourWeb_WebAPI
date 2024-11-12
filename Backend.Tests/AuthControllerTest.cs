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
using System.Text;
using BookingTourWeb_WebAPI.Models.InputModels;
using System.Net;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests
{
    public class AuthControllerTest
    {
        private readonly AuthController _controller;
        private readonly DVMayBayContext _context;
        private readonly DbContextOptions<DVMayBayContext> _contextOptions;
        private readonly IConfiguration _configuration;

        public AuthControllerTest()
        {
            // Sử dụng InMemory Database cho kiểm thử
            _contextOptions = new DbContextOptionsBuilder<DVMayBayContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .EnableSensitiveDataLogging()
                .Options;

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"Jwt:Key", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImN1b25nIiwibmJmIjoxNDc3NTY1NzI0LCJleHAiOjE0Nzc1NjY5MjQsImlhdCI6MTQ3NzU2NTcyNH0.6MzD1VwA5AcOcajkFyKhLYybr3h13iZjDyHm9zysDFQ"}, // Key bí mật
                {"Jwt:Issuer", "http://localhost:28747/"}, // Issuer
                {"Jwt:Audience", "http://localhost:28747/"}, // Audience
            })
            .Build();
            _configuration = configuration;


            _context = new DVMayBayContext(_contextOptions);

            // Seed dữ liệu mẫu cho database
            SeedDatabase();

            _controller = new AuthController(_context, _configuration);
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

        private void SeedDatabase()
        {
            _context.Taikhoans.RemoveRange(_context.Taikhoans);
            _context.Khachhangs.RemoveRange(_context.Khachhangs);
            _context.SaveChanges();
            // Thêm dữ liệu mẫu vào database

            _context.Taikhoans.AddRange(new List<Taikhoan>
            {
                new Taikhoan { MaTaiKhoan = 1, MatKhau = ToHash("123"), TaiKhoan1 = "trung", VaiTro = 0},
                new Taikhoan { MaTaiKhoan = 2, MatKhau = ToHash("123"), TaiKhoan1 = "nam", VaiTro = 0},
                new Taikhoan { MaTaiKhoan = 3, MatKhau = ToHash("123"), TaiKhoan1 = "minh", VaiTro = 0},
            });
            _context.SaveChanges();


            _context.Khachhangs.AddRange(new List<Khachhang>
            {
                new Khachhang { MaKh = 1, MaTaiKhoan = 1, GmailKh = "trunghoangnguyen.it@gmail.com", Phai = "Nam", Sdt = "123456789", TenKh = "trung" },
                new Khachhang { MaKh = 2, MaTaiKhoan = 2, GmailKh = "nhatnam@gmail.com", Phai = "Nam", Sdt = "123456789", TenKh = "Nam" },
                new Khachhang { MaKh = 3, MaTaiKhoan = 3, GmailKh = "khanhminh@gmail.com", Phai = "Nam", Sdt = "123456789", TenKh = "Minh" },
            });
            _context.SaveChanges();
        }

        [Fact]
        public async Task LoginAsync_ReturnsToken_WithValidCredentials()
        {
            // Arrange
            var validUser = new Taikhoan { TaiKhoan1 = "testuser", MatKhau = ToHash("password"), MaTaiKhoan = 4, VaiTro = 0 };
            _context.Taikhoans.Add(validUser);
            await _context.SaveChangesAsync();

            var loginRequest = new InputLogin { TaiKhoan1 = "testuser", MatKhau = "password" };


            // Act
            var result = await _controller.LoginAsync(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResult.Value);
            Assert.NotNull(token);
            Assert.NotEmpty(token); // Kiểm tra token không rỗng
        }


        [Fact]
        public async Task LoginAsync_ReturnsFalse_WithInvalidCredentials()
        {
            // Arrange
            var loginRequest = new InputLogin { TaiKhoan1 = "invaliduser", MatKhau = "password" };


            // Act
            var result = await _controller.LoginAsync(loginRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task LoginAsync_ReturnsBadRequest_WithNonExistingUser()
        {

            // Arrange

            var loginRequest = new InputLogin { TaiKhoan1 = "nonexistinguser", MatKhau = "password" }; // User không tồn tại

            // Act
            var result = await _controller.LoginAsync(loginRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);

        }

        [Fact]
        public async Task LoginAsync_ReturnsBadRequest_WithNullRequest()
        {
            // Act
            var result = await _controller.LoginAsync(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
