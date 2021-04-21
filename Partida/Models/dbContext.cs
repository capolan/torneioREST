using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Partida.Models
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Jogadore> Jogadores { get; set; }
        public virtual DbSet<Jogo> Jogos { get; set; }
        public virtual DbSet<Time> Times { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=TimesDB", Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.23-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Jogadore>(entity =>
            {
                entity.ToTable("jogadores");

                entity.HasIndex(e => e.TimesId, "fk_time_jog_idx");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasColumnType("varchar(255)")
                    .HasColumnName("nome")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");

                entity.Property(e => e.TimesId).HasColumnName("times_id");

                entity.HasOne(d => d.Times)
                    .WithMany(p => p.Jogadoress)
                    .HasForeignKey(d => d.TimesId)
                    .HasConstraintName("fk_time_jog");
            });

            modelBuilder.Entity<Jogo>(entity =>
            {
                entity.ToTable("jogos");

                entity.HasIndex(e => e.Time1Id, "fk_time1_idx");

                entity.HasIndex(e => e.Time2Id, "fk_time2_idx");

                entity.HasIndex(e => new { e.Time1Id, e.Time2Id }, "uniq_jogos")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CriadoEm)
                    .HasColumnType("datetime")
                    .HasColumnName("criado_em");

                entity.Property(e => e.Time1Gol).HasColumnName("time1_gol");

                entity.Property(e => e.Time1Id).HasColumnName("time1_id");

                entity.Property(e => e.Time2Gol).HasColumnName("time2_gol");

                entity.Property(e => e.Time2Id).HasColumnName("time2_id");

                entity.HasOne(d => d.Time1)
                    .WithMany(p => p.JogoTime1s)
                    .HasForeignKey(d => d.Time1Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_time1");

                entity.HasOne(d => d.Time2)
                    .WithMany(p => p.JogoTime2s)
                    .HasForeignKey(d => d.Time2Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_time2");
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.ToTable("times");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CriadoEm)
                    .HasColumnType("datetime")
                    .HasColumnName("criado_em");

                entity.Property(e => e.Jogadores)
                    .HasColumnName("Jogadores")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Nome)
                    .HasColumnType("varchar(255)")
                    .HasColumnName("nome")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
