using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace NeroStore.Models
{
    public partial class NeroStoreDBContext : DbContext
    {
        public NeroStoreDBContext()
        {
        }

        public NeroStoreDBContext(DbContextOptions<NeroStoreDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kayttaja> Kayttajas { get; set; }
        public virtual DbSet<Nayttokerrat> Nayttokerrats { get; set; }
        public virtual DbSet<Tilau> Tilaus { get; set; }
        public virtual DbSet<TilausRivi> TilausRivis { get; set; }
        public virtual DbSet<Tuote> Tuotes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=localhost;database=NeroStoreDB;trusted_connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Kayttaja>(entity =>
            {
                entity.ToTable("Kayttaja");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Etunimi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("etunimi");

                entity.Property(e => e.OnAdmin).HasColumnName("onAdmin");

                entity.Property(e => e.Osoite)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("osoite");

                entity.Property(e => e.Postinumero)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("postinumero")
                    .IsFixedLength(true);

                entity.Property(e => e.Salasana)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("salasana");

                entity.Property(e => e.Sukunimi)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("sukunimi");

                entity.Property(e => e.Syntymäaika)
                    .HasColumnType("date")
                    .HasColumnName("syntymäaika");
            });

            modelBuilder.Entity<Nayttokerrat>(entity =>
            {
                entity.HasKey(e => e.TuoteId)
                    .HasName("PK__Nayttoke__A33385CD40474026");

                entity.ToTable("Nayttokerrat");

                entity.Property(e => e.TuoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("tuote_id");

                entity.Property(e => e.Lkm).HasColumnName("lkm");

                entity.HasOne(d => d.Tuote)
                    .WithOne(p => p.Nayttokerrat)
                    .HasForeignKey<Nayttokerrat>(d => d.TuoteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Nayttokerrat_Tuote");
            });

            modelBuilder.Entity<Tilau>(entity =>
            {
                entity.HasKey(e => e.TilausId)
                    .HasName("PK__Tilaus__0775FE4DD1D155CE");

                entity.Property(e => e.TilausId).HasColumnName("tilaus_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Tilauspvm)
                    .HasColumnType("date")
                    .HasColumnName("tilauspvm");

                entity.Property(e => e.Tilaussumma)
                    .HasColumnType("decimal(7, 2)")
                    .HasColumnName("tilaussumma");

                entity.Property(e => e.ToimitusPvm)
                    .HasColumnType("date")
                    .HasColumnName("toimitusPvm");
            });

            modelBuilder.Entity<TilausRivi>(entity =>
            {
                entity.ToTable("TilausRivi");

                entity.Property(e => e.TilausriviId).HasColumnName("tilausrivi_id");

                entity.Property(e => e.Lkm).HasColumnName("lkm");

                entity.Property(e => e.TilausId).HasColumnName("tilaus_id");

                entity.Property(e => e.TuoteId).HasColumnName("tuote_id");

                entity.HasOne(d => d.Tilaus)
                    .WithMany(p => p.TilausRivis)
                    .HasForeignKey(d => d.TilausId)
                    .HasConstraintName("FK__TilausRiv__tilau__34C8D9D1");

                entity.HasOne(d => d.Tuote)
                    .WithMany(p => p.TilausRivis)
                    .HasForeignKey(d => d.TuoteId)
                    .HasConstraintName("FK__TilausRiv__tuote__35BCFE0A");
            });

            modelBuilder.Entity<Tuote>(entity =>
            {
                entity.ToTable("Tuote");

                entity.Property(e => e.TuoteId).HasColumnName("tuote_id");

                entity.Property(e => e.Hinta)
                    .HasColumnType("decimal(7, 2)")
                    .HasColumnName("hinta");

                entity.Property(e => e.Kuvaus)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("kuvaus");

                entity.Property(e => e.Lkm).HasColumnName("lkm");

                entity.Property(e => e.Nimi)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nimi");

                entity.Property(e => e.Tuoteryhma)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tuoteryhma");

                entity.Property(e => e.Tyyppi)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("tyyppi");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
