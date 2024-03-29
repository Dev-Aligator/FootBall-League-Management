﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Football_League_App.Models;

public partial class FlmdbContext : DbContext
{
    public FlmdbContext()
    {
    }

    public FlmdbContext(DbContextOptions<FlmdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<League> Leagues { get; set; }

    public virtual DbSet<Match> Matchs { get; set; }

    public virtual DbSet<MatchDetail> MatchDetails { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=FLMDB;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.MaThe).HasName("PK_MaThe");

            entity.ToTable(tb => tb.HasTrigger("Cards_PhutBuGio"));

            entity.Property(e => e.MaThe)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('Card'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
				.ValueGeneratedNever(); //new
			entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.MaCtnhanThe)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MaCTNhanThe");
            entity.Property(e => e.MaTd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("MaTD");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.MaClb).HasName("PK_MaCLB");

            entity.Property(e => e.MaClb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('Club'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaCLB")
				.ValueGeneratedNever(); //new
			entity.Property(e => e.DiaChi).HasMaxLength(40);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.MaCt)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MaCT");
            entity.Property(e => e.TenClb)
                .HasMaxLength(20)
                .HasColumnName("TenCLB");
            entity.Property(e => e.TenSvd)
                .HasMaxLength(20)
                .HasColumnName("TenSVD");
            entity.Property(e => e.ImgPath)
                .HasMaxLength(150)
                .HasColumnName("Img_File");

            entity.HasOne(d => d.MaCtNavigation).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.MaCt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MACT");
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.MaBt).HasName("PK_MaBT");

            entity.ToTable(tb => tb.HasTrigger("Goals_PhutBuGio"));

            entity.Property(e => e.MaBt)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('Goal'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaBT")
				.ValueGeneratedNever(); //new
			entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.LoaiBt)
                .HasMaxLength(30)
                .HasColumnName("LoaiBT");
            entity.Property(e => e.MaCtghiBan)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MaCTGhiBan");
            entity.Property(e => e.MaCtkienTao)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasColumnName("MaCTKienTao");
            entity.Property(e => e.MaTd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("MaTD");

            entity.HasOne(d => d.MaTdNavigation).WithMany(p => p.Goals)
                .HasForeignKey(d => d.MaTd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MATD");
        });

        modelBuilder.Entity<League>(entity =>
        {
            entity.HasKey(e => e.MaLeague).HasName("PK_MaLeague");

            entity.ToTable("League");

            entity.HasIndex(e => e.LeagueName, "UQ__League__02D6A5720B7DB1AA").IsUnique();

            entity.Property(e => e.MaLeague)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasComputedColumnSql("('League'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
				.ValueGeneratedNever(); //new
			entity.Property(e => e.EndDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.LeagueName).HasMaxLength(15);
            entity.Property(e => e.MaRule);
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Rules>(entity =>
        {
            entity.HasKey(e => e.MaRules).HasName("PK_MaRule");
            entity.Property(e => e.BasicInfor)
            .HasMaxLength(200)
            .IsUnicode(false);
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(e => e.MaTd).HasName("PK_MaTD");

            entity.Property(e => e.MaTd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasComputedColumnSql("('Match'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaTD")
				.ValueGeneratedNever(); //new
			entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.MaDoiKhach)
                .HasMaxLength(7)
                .IsUnicode(false);
            entity.Property(e => e.MaDoiNha)
                .HasMaxLength(7)
                .IsUnicode(false);

            entity.HasOne(d => d.MaDoiKhachNavigation).WithMany(p => p.MatchMaDoiKhachNavigations)
                .HasForeignKey(d => d.MaDoiKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MADOIKHACH");

            entity.HasOne(d => d.MaDoiNhaNavigation).WithMany(p => p.MatchMaDoiNhaNavigations)
                .HasForeignKey(d => d.MaDoiNha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MADOINHA");
        });

        modelBuilder.Entity<MatchDetail>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.MaCttd)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComputedColumnSql("('MDT'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaCTTD")
				.ValueGeneratedNever(); //new
			entity.Property(e => e.MaTd)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("MaTD");

            entity.HasOne(d => d.MaTdNavigation).WithMany()
                .HasForeignKey(d => d.MaTd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MATD_CTTD");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.MaCt).HasName("PK_MaCT");

            entity.Property(e => e.MaCt)
                .HasMaxLength(9)
                .IsUnicode(false)
                .HasComputedColumnSql("('Player'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaCT")
				.ValueGeneratedNever(); //new
			entity.Property(e => e.ChanThuan).HasMaxLength(30);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.LoaiCt).HasColumnName("LoaiCT");
            entity.Property(e => e.Luong).HasColumnType("int");
            entity.Property(e => e.MaClb)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("MaCLB");
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenCt)
                .HasMaxLength(30)
                .HasColumnName("TenCT");
            entity.Property(e => e.ViTriChinh).HasMaxLength(30);
            entity.Property(e => e.ViTriPhu).HasMaxLength(30);

            entity.HasOne(d => d.MaClbNavigation).WithMany(p => p.Players)
                .HasForeignKey(d => d.MaClb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MACLB");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaUsers).HasName("PK_MaUsers");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F284564AEB2437").IsUnique();

            entity.Property(e => e.MaUsers)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('User'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
				.ValueGeneratedNever(); //new
			entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SDT");
            entity.Property(e => e.UserName).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
