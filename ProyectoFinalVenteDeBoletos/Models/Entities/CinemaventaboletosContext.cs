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

    public virtual DbSet<Asiento> Asiento { get; set; }

    public virtual DbSet<Boleto> Boleto { get; set; }

    public virtual DbSet<Clasificacion> Clasificacion { get; set; }

    public virtual DbSet<Genero> Genero { get; set; }

    public virtual DbSet<Horario> Horario { get; set; }

    public virtual DbSet<Pelicula> Pelicula { get; set; }

    public virtual DbSet<PeliculaGenero> PeliculaGenero { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Sala> Sala { get; set; }

    public virtual DbSet<Tipopantalla> Tipopantalla { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;database=cinemaventaboletos;username=Cinema;password=Cinema", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Asiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asiento");

            entity.Property(e => e.Seleccionado).HasDefaultValueSql("'0'");
        });

        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("boleto");

            entity.HasIndex(e => e.IdAsiento, "FK_Boleto_Asiento");

            entity.HasIndex(e => e.IdHorario, "FK_Boleto_Horario");

            entity.Property(e => e.IdAsiento).HasColumnName("Id_Asiento");
            entity.Property(e => e.IdHorario).HasColumnName("Id_Horario");

            entity.HasOne(d => d.IdAsientoNavigation).WithMany(p => p.Boleto)
                .HasForeignKey(d => d.IdAsiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boleto_Asiento");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Boleto)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boleto_Horario");
        });

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

            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("Hora_Inicio");
            entity.Property(e => e.HoraTerminacion)
                .HasColumnType("time")
                .HasColumnName("Hora_Terminacion");
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

            entity.HasIndex(e => e.IdClasificacion, "Fk_Pelicula_Clasificacion");

            entity.Property(e => e.Duracion).HasColumnType("time");
            entity.Property(e => e.IdClasificacion).HasColumnName("Id_Clasificacion");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Sinopsis).HasColumnType("text");
            entity.Property(e => e.Trailer).HasMaxLength(48);

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Pelicula)
                .HasForeignKey(d => d.IdClasificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Pelicula_Clasificacion");
        });

        modelBuilder.Entity<PeliculaGenero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula_genero");

            entity.HasIndex(e => e.IdGenero, "pelicula_genero_Genero");

            entity.HasIndex(e => e.IdPelicula, "pelicula_genero_Pelicula");

            entity.Property(e => e.IdGenero).HasColumnName("Id_Genero");
            entity.Property(e => e.IdPelicula).HasColumnName("Id_Pelicula");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.PeliculaGenero)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_genero_Genero");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculaGenero)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_genero_Pelicula");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(13);
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.IdTipoPantalla, "Fk_Sala_TipoPantalla");

            entity.Property(e => e.IdTipoPantalla).HasColumnName("Id_TipoPantalla");

            entity.HasOne(d => d.IdTipoPantallaNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.IdTipoPantalla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_Sala_TipoPantalla");
        });

        modelBuilder.Entity<Tipopantalla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipopantalla");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(5);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.HasIndex(e => e.IdBoleto, "fk_Usuario_Boleto");

            entity.HasIndex(e => e.IdRol, "fk_Usuario_Rol");

            entity.Property(e => e.Contraseña).HasMaxLength(128);
            entity.Property(e => e.IdBoleto).HasColumnName("Id_Boleto");
            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Username).HasMaxLength(5);

            entity.HasOne(d => d.IdBoletoNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdBoleto)
                .HasConstraintName("fk_Usuario_Boleto");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("fk_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
