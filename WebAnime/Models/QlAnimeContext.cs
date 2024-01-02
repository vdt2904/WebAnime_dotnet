using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebAnime.Models;

public partial class QlAnimeContext : DbContext
{
    public QlAnimeContext()
    {
    }

    public QlAnimeContext(DbContextOptions<QlAnimeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<TbAnime> TbAnimes { get; set; }

    public virtual DbSet<TbBlog> TbBlogs { get; set; }

    public virtual DbSet<TbComment> TbComments { get; set; }

    public virtual DbSet<TbDoanhThu> TbDoanhThus { get; set; }

    public virtual DbSet<TbHangPhim> TbHangPhims { get; set; }

    public virtual DbSet<TbHoaDon> TbHoaDons { get; set; }

    public virtual DbSet<TbLoaiPhim> TbLoaiPhims { get; set; }

    public virtual DbSet<TbNguoiDung> TbNguoiDungs { get; set; }

    public virtual DbSet<TbOurBlog> TbOurBlogs { get; set; }

    public virtual DbSet<TbResetPass> TbResetPasses { get; set; }

    public virtual DbSet<TbReview> TbReviews { get; set; }

    public virtual DbSet<TbTapPhim> TbTapPhims { get; set; }

    public virtual DbSet<TbTheLoai> TbTheLoais { get; set; }

    public virtual DbSet<TbTlanime> TbTlanimes { get; set; }

    public virtual DbSet<TbView> TbViews { get; set; }

    public virtual DbSet<TbVip> TbVips { get; set; }

    public virtual DbSet<TbWorth> TbWorths { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=VDT\\SQLEXPRESS;Initial Catalog=QlAnime;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<TbAnime>(entity =>
        {
            entity.HasKey(e => e.MaAnime);

            entity.ToTable("tb_Anime");

            entity.Property(e => e.MaAnime).HasMaxLength(10);
            entity.Property(e => e.Anh).HasMaxLength(500);
            entity.Property(e => e.Anime).HasMaxLength(50);
            entity.Property(e => e.Lp).HasColumnName("LP");
            entity.Property(e => e.MaHp)
                .HasMaxLength(10)
                .HasColumnName("MaHP");
            entity.Property(e => e.MaLp)
                .HasMaxLength(10)
                .HasColumnName("MaLP");
            entity.Property(e => e.NgayPhatSong).HasColumnType("datetime");
            entity.Property(e => e.ThongTin).HasMaxLength(2000);

            entity.HasOne(d => d.MaHpNavigation).WithMany(p => p.TbAnimes)
                .HasForeignKey(d => d.MaHp)
                .HasConstraintName("FK_tb_Anime_tb_HangPhim");

            entity.HasOne(d => d.MaLpNavigation).WithMany(p => p.TbAnimes)
                .HasForeignKey(d => d.MaLp)
                .HasConstraintName("FK_tb_Anime_tb_LoaiPhim");
        });

        modelBuilder.Entity<TbBlog>(entity =>
        {
            entity.ToTable("tb_Blog");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idblog).HasColumnName("IDBlog");
            entity.Property(e => e.MaAnime).HasMaxLength(10);
            entity.Property(e => e.Trailer).HasMaxLength(500);

            entity.HasOne(d => d.IdblogNavigation).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.Idblog)
                .HasConstraintName("FK_tb_Blog_tb_OurBlog");

            entity.HasOne(d => d.MaAnimeNavigation).WithMany(p => p.TbBlogs)
                .HasForeignKey(d => d.MaAnime)
                .HasConstraintName("FK_tb_Blog_tb_Anime");
        });

        modelBuilder.Entity<TbComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tb_Comment_1");

            entity.ToTable("tb_Comment", tb => tb.HasTrigger("trg_updateComment"));

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.MaTp)
                .HasMaxLength(10)
                .HasColumnName("MaTP");
            entity.Property(e => e.NgayComent).HasColumnType("datetime");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.TbComments)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK_tb_Comment_tb_NguoiDung");

            entity.HasOne(d => d.MaTpNavigation).WithMany(p => p.TbComments)
                .HasForeignKey(d => d.MaTp)
                .HasConstraintName("FK_tb_Comment_tb_TapPhim");
        });

        modelBuilder.Entity<TbDoanhThu>(entity =>
        {
            entity.ToTable("tb_DoanhThu");

            entity.Property(e => e.Id).HasColumnName("ID");
        });

        modelBuilder.Entity<TbHangPhim>(entity =>
        {
            entity.HasKey(e => e.MaHp);

            entity.ToTable("tb_HangPhim");

            entity.Property(e => e.MaHp)
                .HasMaxLength(10)
                .HasColumnName("MaHP");
            entity.Property(e => e.TenHangPhim).HasMaxLength(50);
        });

        modelBuilder.Entity<TbHoaDon>(entity =>
        {
            entity.HasKey(e => e.SoHd);

            entity.ToTable("tb_HoaDon");

            entity.Property(e => e.SoHd)
                .HasMaxLength(10)
                .HasColumnName("SoHD");
            entity.Property(e => e.LoaiVip).HasMaxLength(10);
            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.NgayHetHan).HasColumnType("date");
            entity.Property(e => e.NgayMua).HasColumnType("date");

            entity.HasOne(d => d.LoaiVipNavigation).WithMany(p => p.TbHoaDons)
                .HasForeignKey(d => d.LoaiVip)
                .HasConstraintName("FK_tb_HoaDon_tb_VIP");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.TbHoaDons)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK_tb_HoaDon_tb_NguoiDung");
        });

        modelBuilder.Entity<TbLoaiPhim>(entity =>
        {
            entity.HasKey(e => e.MaLp);

            entity.ToTable("tb_LoaiPhim");

            entity.Property(e => e.MaLp)
                .HasMaxLength(10)
                .HasColumnName("MaLP");
            entity.Property(e => e.LoaiPhim).HasMaxLength(50);
        });

        modelBuilder.Entity<TbNguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNd);

            entity.ToTable("tb_NguoiDung");

            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.LoaiNd).HasColumnName("LoaiND");
            entity.Property(e => e.Pasword).HasMaxLength(50);
            entity.Property(e => e.Sdt)
                .HasMaxLength(50)
                .HasColumnName("SDT");
            entity.Property(e => e.TenNguoiDung).HasMaxLength(50);
        });

        modelBuilder.Entity<TbOurBlog>(entity =>
        {
            entity.HasKey(e => e.Idblog);

            entity.ToTable("tb_OurBlog");

            entity.Property(e => e.Idblog).HasColumnName("IDBlog");
            entity.Property(e => e.Anh)
                .HasMaxLength(500)
                .HasColumnName("anh");
            entity.Property(e => e.NgayDang).HasColumnType("date");
            entity.Property(e => e.TenBlog).HasMaxLength(500);
        });

        modelBuilder.Entity<TbResetPass>(entity =>
        {
            entity.ToTable("tb_ResetPass");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(500);
            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.ResetPass).HasMaxLength(500);
            entity.Property(e => e.ThoiGian).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(50);

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.TbResetPasses)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK_tb_ResetPass_tb_NguoiDung");
        });

        modelBuilder.Entity<TbReview>(entity =>
        {
            entity.ToTable("tb_Review");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaAnime).HasMaxLength(10);
            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.NgayReview).HasColumnType("datetime");

            entity.HasOne(d => d.MaAnimeNavigation).WithMany(p => p.TbReviews)
                .HasForeignKey(d => d.MaAnime)
                .HasConstraintName("FK_tb_Review_tb_Anime");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.TbReviews)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK_tb_Review_tb_NguoiDung");
        });

        modelBuilder.Entity<TbTapPhim>(entity =>
        {
            entity.HasKey(e => e.MaTp);

            entity.ToTable("tb_TapPhim");

            entity.Property(e => e.MaTp)
                .HasMaxLength(10)
                .HasColumnName("MaTP");
            entity.Property(e => e.AnhVideo).HasMaxLength(50);
            entity.Property(e => e.MaAnime).HasMaxLength(10);
            entity.Property(e => e.NgayPhatSong).HasColumnType("datetime");
            entity.Property(e => e.Video).HasMaxLength(50);

            entity.HasOne(d => d.MaAnimeNavigation).WithMany(p => p.TbTapPhims)
                .HasForeignKey(d => d.MaAnime)
                .HasConstraintName("FK_tb_TapPhim_tb_Anime");
        });

        modelBuilder.Entity<TbTheLoai>(entity =>
        {
            entity.HasKey(e => e.MaTl);

            entity.ToTable("tb_TheLoai");

            entity.Property(e => e.MaTl)
                .HasMaxLength(10)
                .HasColumnName("MaTL");
            entity.Property(e => e.TheLoai).HasMaxLength(50);
            entity.Property(e => e.ThongTin).HasMaxLength(1000);
        });

        modelBuilder.Entity<TbTlanime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tb_TL_Anime");

            entity.ToTable("tb_TLAnime");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaAnime).HasMaxLength(10);
            entity.Property(e => e.MaTl)
                .HasMaxLength(10)
                .HasColumnName("MaTL");

            entity.HasOne(d => d.MaAnimeNavigation).WithMany(p => p.TbTlanimes)
                .HasForeignKey(d => d.MaAnime)
                .HasConstraintName("FK_tb_TL_Anime_tb_Anime");

            entity.HasOne(d => d.MaTlNavigation).WithMany(p => p.TbTlanimes)
                .HasForeignKey(d => d.MaTl)
                .HasConstraintName("FK_tb_TL_Anime_tb_TheLoai");
        });

        modelBuilder.Entity<TbView>(entity =>
        {
            entity.ToTable("tb_View");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNd)
                .HasMaxLength(10)
                .HasColumnName("MaND");
            entity.Property(e => e.MaTp)
                .HasMaxLength(10)
                .HasColumnName("MaTP");
            entity.Property(e => e.NgayXem).HasColumnType("datetime");
            entity.Property(e => e.Slviews).HasColumnName("SLViews");

            entity.HasOne(d => d.MaNdNavigation).WithMany(p => p.TbViews)
                .HasForeignKey(d => d.MaNd)
                .HasConstraintName("FK_tb_View_tb_NguoiDung");

            entity.HasOne(d => d.MaTpNavigation).WithMany(p => p.TbViews)
                .HasForeignKey(d => d.MaTp)
                .HasConstraintName("FK_tb_View_tb_TapPhim");
        });

        modelBuilder.Entity<TbVip>(entity =>
        {
            entity.HasKey(e => e.LoaiVip).HasName("PK_tb_VIP_1");

            entity.ToTable("tb_VIP");

            entity.Property(e => e.LoaiVip).HasMaxLength(10);
            entity.Property(e => e.ThoiGianSd).HasColumnName("ThoiGianSD");
        });

        modelBuilder.Entity<TbWorth>(entity =>
        {
            entity.ToTable("tb_Worth");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaAnime).HasMaxLength(10);

            entity.HasOne(d => d.MaAnimeNavigation).WithMany(p => p.TbWorths)
                .HasForeignKey(d => d.MaAnime)
                .HasConstraintName("FK_tb_Worth_tb_Anime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
