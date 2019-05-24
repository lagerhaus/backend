using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LagerhausDb
{
    public partial class LagerhausContext : DbContext
    {
        public LagerhausContext()
        {
        }

        public LagerhausContext(DbContextOptions<LagerhausContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Batch> Batch { get; set; }
        public virtual DbSet<Fruit> Fruit { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Ripeness> Ripeness { get; set; }
        public virtual DbSet<Weather> Weather { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>(entity =>
            {
                entity.ToTable("batch");

                entity.HasIndex(e => new { e.FruitId, e.Year, e.Month })
                    .HasName("batch_fruit_id_year_month_key")
                    .IsUnique();

                entity.Property(e => e.BatchId)
                    .HasColumnName("batch_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.Property(e => e.RipenessId).HasColumnName("ripeness_id");

                entity.Property(e => e.StorageDate)
                    .HasColumnName("storage_date")
                    .HasColumnType("date");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.Batch)
                    .HasForeignKey(d => d.FruitId)
                    .HasConstraintName("batch_fruit_id_fkey");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Batch)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("batch_region_id_fkey");

                entity.HasOne(d => d.Ripeness)
                    .WithMany(p => p.Batch)
                    .HasForeignKey(d => d.RipenessId)
                    .HasConstraintName("batch_ripeness_id_fkey");
            });

            modelBuilder.Entity<Fruit>(entity =>
            {
                entity.ToTable("fruit");

                entity.HasIndex(e => e.Name)
                    .HasName("fruit_name_key")
                    .IsUnique();

                entity.Property(e => e.FruitId)
                    .HasColumnName("fruit_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("region");

                entity.HasIndex(e => e.Name)
                    .HasName("region_name_key")
                    .IsUnique();

                entity.Property(e => e.RegionId)
                    .HasColumnName("region_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Area)
                    .HasColumnName("area")
                    .HasColumnType("character varying");

                entity.Property(e => e.Level).HasColumnName("level");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Ripeness>(entity =>
            {
                entity.ToTable("ripeness");

                entity.HasIndex(e => new { e.Name, e.FruitId })
                    .HasName("ripeness_name_fruit_id_key")
                    .IsUnique();

                entity.Property(e => e.RipenessId)
                    .HasColumnName("ripeness_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.FruitId).HasColumnName("fruit_id");

                entity.Property(e => e.MinimumStorageSpan).HasColumnName("minimum_storage_span");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Fruit)
                    .WithMany(p => p.Ripeness)
                    .HasForeignKey(d => d.FruitId)
                    .HasConstraintName("ripeness_fruit_id_fkey");
            });

            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("weather");

                entity.HasIndex(e => new { e.RegionId, e.Year, e.Month })
                    .HasName("weather_region_id_year_month_key")
                    .IsUnique();

                entity.Property(e => e.WeatherId)
                    .HasColumnName("weather_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Month).HasColumnName("month");

                entity.Property(e => e.RainyDays).HasColumnName("rainy_days");

                entity.Property(e => e.RegionId).HasColumnName("region_id");

                entity.Property(e => e.SunnyDays).HasColumnName("sunny_days");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Weather)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("weather_region_id_fkey");
            });
        }
    }
}
