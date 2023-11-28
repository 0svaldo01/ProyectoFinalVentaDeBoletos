using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalVenteDeBoletos.Models.Entities;

public partial class CinemaventaboletosContext : DbContext
{
    public CinemaventaboletosContext()
    {
    }

    public CinemaventaboletosContext(DbContextOptions<CinemaventaboletosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Boletosvendidos> Boletosvendidos { get; set; }

    public virtual DbSet<Clasifiacion> Clasifiacion { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Horario> Horario { get; set; }

    public virtual DbSet<Peliculas> Peliculas { get; set; }

    public virtual DbSet<Salas> Salas { get; set; }

    public virtual DbSet<Tipousuarios> Tipousuarios { get; set; }

    public virtual DbSet<Transacciones> Transacciones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=cinemaventaboletos;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Boletosvendidos>(entity =>
        {
            entity.HasKey(e => e.IdBoletosVendidos).HasName("PRIMARY");

            entity.ToTable("boletosvendidos");

            entity.HasIndex(e => e.IdTransacciones, "fk_BoletosVendidos_Transacciones1_idx");

            entity.Property(e => e.IdBoletosVendidos).HasColumnName("Id_BoletosVendidos");
            entity.Property(e => e.Estado).HasMaxLength(45);
            entity.Property(e => e.IdTransacciones).HasColumnName("Id_Transacciones");
            entity.Property(e => e.NumeroAsientol).HasMaxLength(45);

            entity.HasOne(d => d.IdTransaccionesNavigation).WithMany(p => p.Boletosvendidos)
                .HasForeignKey(d => d.IdTransacciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_BoletosVendidos_Transacciones1");
        });

        modelBuilder.Entity<Clasifiacion>(entity =>
        {
            entity.HasKey(e => e.IdClasifiacion).HasName("PRIMARY");

            entity.ToTable("clasifiacion");

            entity.Property(e => e.IdClasifiacion).HasColumnName("Id_Clasifiacion");
            entity.Property(e => e.Edad).HasMaxLength(45);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PRIMARY");

            entity.ToTable("genero");

            entity.Property(e => e.IdGenero).HasColumnName("Id_Genero");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PRIMARY");

            entity.ToTable("horario");

            entity.HasIndex(e => e.IdPelicula, "fk_Horario_Peliculas_idx");

            entity.HasIndex(e => e.IdSala, "fk_Horario_Salas1_idx");

            entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
            entity.Property(e => e.FechaHora)
                .HasColumnType("datetime")
                .HasColumnName("Fecha/Hora");
            entity.Property(e => e.IdPelicula).HasColumnName("Id_Pelicula");
            entity.Property(e => e.IdSala).HasColumnName("Id_Sala");
            entity.Property(e => e.Precio).HasPrecision(10);

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Horario_Peliculas");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Horario_Salas1");
        });

        modelBuilder.Entity<Peliculas>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("peliculas");

            entity.HasIndex(e => e.IdClasifiacion, "fk_Peliculas_Clasifiacion1_idx");

            entity.HasIndex(e => e.IdGenero, "fk_Peliculas_Genero1_idx");

            entity.Property(e => e.Duracion).HasPrecision(45);
            entity.Property(e => e.IdClasifiacion).HasColumnName("Id_Clasifiacion");
            entity.Property(e => e.IdGenero).HasColumnName("Id_Genero");
            entity.Property(e => e.Nombre).HasMaxLength(45);
            entity.Property(e => e.Sinopsis).HasMaxLength(45);
            entity.Property(e => e.Trailer).HasMaxLength(45);

            entity.HasOne(d => d.IdClasifiacionNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdClasifiacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Peliculas_Clasifiacion1");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Peliculas_Genero1");
        });

        modelBuilder.Entity<Salas>(entity =>
        {
            entity.HasKey(e => e.IdSala).HasName("PRIMARY");

            entity.ToTable("salas");

            entity.Property(e => e.IdSala).HasColumnName("Id_Sala");
            entity.Property(e => e.IdTipoPantalla).HasColumnName("Id_TipoPantalla");
        });

        modelBuilder.Entity<Tipousuarios>(entity =>
        {
            entity.HasKey(e => e.IdTipoUsuarios).HasName("PRIMARY");

            entity.ToTable("tipousuarios");

            entity.Property(e => e.IdTipoUsuarios).HasColumnName("Id_TipoUsuarios");
        });

        modelBuilder.Entity<Transacciones>(entity =>
        {
            entity.HasKey(e => e.IdTransacciones).HasName("PRIMARY");

            entity.ToTable("transacciones");

            entity.HasIndex(e => e.IdHorario, "fk_Transacciones_Horario1_idx");

            entity.HasIndex(e => e.IdSala, "fk_Transacciones_Salas1_idx");

            entity.HasIndex(e => e.IdTipoUsuarios, "fk_Transacciones_TipoUsuarios1_idx");

            entity.Property(e => e.IdTransacciones).HasColumnName("Id_Transacciones");
            entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");
            entity.Property(e => e.IdSala).HasColumnName("Id_Sala");
            entity.Property(e => e.IdTipoUsuarios).HasColumnName("Id_TipoUsuarios");
            entity.Property(e => e.Total).HasPrecision(10);

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Transacciones_Horario1");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Transacciones_Salas1");

            entity.HasOne(d => d.IdTipoUsuariosNavigation).WithMany(p => p.Transacciones)
                .HasForeignKey(d => d.IdTipoUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Transacciones_TipoUsuarios1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
