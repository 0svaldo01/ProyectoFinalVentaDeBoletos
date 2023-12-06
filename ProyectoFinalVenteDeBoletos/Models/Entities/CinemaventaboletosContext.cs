using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class CinemaventaboletosContext : DbContext
{
    public CinemaventaboletosContext()
    {
    }

    public CinemaventaboletosContext(DbContextOptions<CinemaventaboletosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Clasificacion> Clasificacion { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Horario> Horario { get; set; }

    public virtual DbSet<Pelicula> Pelicula { get; set; }

    public virtual DbSet<Sala> Sala { get; set; }

    public virtual DbSet<Tipopantalla> Tipopantalla { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=cinemaventaboletos;username=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Clasificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clasificacion");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genero");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horario");

            entity.HasIndex(e => e.IdPelicula, "FK_Horario_Pelicula");

            entity.HasIndex(e => e.IdSala, "FK_Horario_Sala");

            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Hora");
            entity.Property(e => e.IdPelicula).HasColumnName("Id_Pelicula");
            entity.Property(e => e.IdSala).HasColumnName("Id_Sala");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdPelicula)
                .HasConstraintName("FK_Horario_Pelicula");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdSala)
                .HasConstraintName("FK_Horario_Sala");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula");

            entity.HasIndex(e => e.IdClasificacion, "IdClasificacion");

            entity.HasIndex(e => e.IdGeneros, "IdGeneros");

            entity.Property(e => e.Duracion).HasColumnType("time");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Sinopsis).HasColumnType("text");
            entity.Property(e => e.Trailer).HasMaxLength(48);

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.IdClasificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_ibfk_2");

            entity.HasOne(d => d.IdGenerosNavigation).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.IdGeneros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_ibfk_1");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.IdTipoPantalla, "IdTipoPantalla");

            entity.HasOne(d => d.IdTipoPantallaNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.IdTipoPantalla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sala_ibfk_1");
        });

        modelBuilder.Entity<Tipopantalla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipopantalla");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(5);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
