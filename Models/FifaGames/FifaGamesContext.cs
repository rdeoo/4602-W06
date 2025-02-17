using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentFunctions.Models.FifaGames;

public partial class FifaGamesContext : DbContext
{
    public FifaGamesContext()
    {
    }

    public FifaGamesContext(DbContextOptions<FifaGamesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__games__2AB897FD4CFCD02F");

            entity.ToTable("games");

            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Continent)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Created)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Winner)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
