using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookingTourWeb_WebAPI.Models
{
    public partial class DVMayBayContext : DbContext
    {
        public DVMayBayContext()
        {
        }

        public DVMayBayContext(DbContextOptions<DVMayBayContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chitietve> Chitietves { get; set; } = null!;
        public virtual DbSet<Chuyenbay> Chuyenbays { get; set; } = null!;
        public virtual DbSet<Hoadon> Hoadons { get; set; } = null!;
        public virtual DbSet<Khachhang> Khachhangs { get; set; } = null!;
        public virtual DbSet<Maybay> Maybays { get; set; } = null!;
        public virtual DbSet<Phuongthucthanhtoan> Phuongthucthanhtoans { get; set; } = null!;
        public virtual DbSet<Taikhoan> Taikhoans { get; set; } = null!;
        public virtual DbSet<Ve> Ves { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:flightdot.database.windows.net,1433;Initial Catalog=DVMayBay;Persist Security Info=False;User ID=mt;Password=MinhTrung2003;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chitietve>(entity =>
            {
                entity.HasKey(e => e.MaCtv)
                    .HasName("PK__CHITIETV__3DCB54E75039A44C");

                entity.ToTable("CHITIETVE");

                entity.Property(e => e.MaCtv).HasColumnName("MaCTV");

                entity.Property(e => e.LoaiVe)
                    .HasMaxLength(5)
                    .IsFixedLength();

                entity.Property(e => e.MaChuyenBay).HasMaxLength(100);

                entity.Property(e => e.TinhTrang)
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang xác nhận')");

                entity.Property(e => e.TongGia).HasColumnType("decimal(16, 4)");

                entity.HasOne(d => d.MaChuyenBayNavigation)
                    .WithMany(p => p.Chitietves)
                    .HasForeignKey(d => d.MaChuyenBay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHITIETVE_CHUYENBAY");

                entity.HasOne(d => d.MaVeNavigation)
                    .WithMany(p => p.Chitietves)
                    .HasForeignKey(d => d.MaVe)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHITIETVE_VE");
            });

            modelBuilder.Entity<Chuyenbay>(entity =>
            {
                entity.HasKey(e => e.MaChuyenBay);

                entity.ToTable("CHUYENBAY");

                entity.Property(e => e.MaChuyenBay).HasMaxLength(100);

                entity.Property(e => e.DonGia).HasColumnType("decimal(15, 4)");

                entity.Property(e => e.GioBay).HasColumnType("time(6)");

                entity.Property(e => e.MaMayBay).HasMaxLength(10);

                entity.Property(e => e.NgayXuatPhat).HasColumnType("date");

                entity.Property(e => e.NoiDen).HasMaxLength(15);

                entity.Property(e => e.NoiXuatPhat).HasMaxLength(15);

                entity.Property(e => e.SoLuongVeBsn).HasColumnName("SoLuongVeBSN");

                entity.Property(e => e.SoLuongVeEco).HasColumnName("SoLuongVeECO");

                entity.HasOne(d => d.MaMayBayNavigation)
                    .WithMany(p => p.Chuyenbays)
                    .HasForeignKey(d => d.MaMayBay)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CHUYENBAY_MAYBAY");
            });

            modelBuilder.Entity<Hoadon>(entity =>
            {
                entity.HasKey(e => e.Idhoadon)
                    .HasName("PK__HOADON__0345E74B8839526E");

                entity.ToTable("HOADON");

                entity.Property(e => e.Idhoadon).HasColumnName("IDHoadon");

                entity.Property(e => e.KieuThanhToan)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaGiaoDich)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");

                entity.Property(e => e.TinhTrangThanhToan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.KieuThanhToanNavigation)
                    .WithMany(p => p.Hoadons)
                    .HasForeignKey(d => d.KieuThanhToan)
                    .HasConstraintName("FK__HOADON__KieuThan__73BA3083");

                entity.HasOne(d => d.MaVeNavigation)
                    .WithMany(p => p.Hoadons)
                    .HasForeignKey(d => d.MaVe)
                    .HasConstraintName("FK__HOADON__MaVe__74AE54BC");
            });

            modelBuilder.Entity<Khachhang>(entity =>
            {
                entity.HasKey(e => e.MaKh);

                entity.ToTable("KHACHHANG");

                entity.HasIndex(e => e.GmailKh, "KHACHHANG_GmailKH")
                    .IsUnique();

                entity.HasIndex(e => e.Sdt, "KHACHHANG_SDT")
                    .IsUnique();

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.GmailKh)
                    .HasMaxLength(50)
                    .HasColumnName("GmailKH");

                entity.Property(e => e.Phai).HasMaxLength(5);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(13)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenKh)
                    .HasMaxLength(50)
                    .HasColumnName("TenKH");

                entity.HasOne(d => d.MaTaiKhoanNavigation)
                    .WithMany(p => p.Khachhangs)
                    .HasForeignKey(d => d.MaTaiKhoan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KHACHHANG_TAIKHOAN");
            });

            modelBuilder.Entity<Maybay>(entity =>
            {
                entity.HasKey(e => e.MaMayBay);

                entity.ToTable("MAYBAY");

                entity.Property(e => e.MaMayBay).HasMaxLength(10);

                entity.Property(e => e.SlgheBsn).HasColumnName("SLGheBSN");

                entity.Property(e => e.SlgheEco).HasColumnName("SLGheECO");

                entity.Property(e => e.TenMayBay).HasMaxLength(20);
            });

            modelBuilder.Entity<Phuongthucthanhtoan>(entity =>
            {
                entity.HasKey(e => e.KieuThanhToan);

                entity.ToTable("PHUONGTHUCTHANHTOAN");

                entity.Property(e => e.KieuThanhToan)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TenKieuThanhToan).HasMaxLength(50);
            });

            modelBuilder.Entity<Taikhoan>(entity =>
            {
                entity.HasKey(e => e.MaTaiKhoan);

                entity.ToTable("TAIKHOAN");

                entity.HasIndex(e => e.TaiKhoan1, "TAIKHOAN_TaiKhoan")
                    .IsUnique();

                entity.Property(e => e.TaiKhoan1)
                    .HasMaxLength(50)
                    .HasColumnName("TaiKhoan");

                entity.Property(e => e.VaiTro).HasDefaultValueSql("((2))");
            });

            modelBuilder.Entity<Ve>(entity =>
            {
                entity.HasKey(e => e.MaVe);

                entity.ToTable("VE");

                entity.Property(e => e.MaKh).HasColumnName("MaKH");

                entity.Property(e => e.NgayDatVe).HasColumnType("datetime");

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.Ves)
                    .HasForeignKey(d => d.MaKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VE_KHACHHANG");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
