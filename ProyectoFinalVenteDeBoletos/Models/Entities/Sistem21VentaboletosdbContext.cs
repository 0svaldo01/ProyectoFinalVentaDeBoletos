using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoFinalVentaDeBoletos.Models.Entities;

public partial class Sistem21VentaboletosdbContext : DbContext
{
    public Sistem21VentaboletosdbContext()
    {
    }

    public Sistem21VentaboletosdbContext(DbContextOptions<Sistem21VentaboletosdbContext> options)
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

    public virtual DbSet<PeliculaHorario> PeliculaHorario { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Sala> Sala { get; set; }

    public virtual DbSet<SalaAsiento> SalaAsiento { get; set; }

    public virtual DbSet<Tipopantalla> Tipopantalla { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sistemas19.com;database=sistem21_ventaboletosdb;username=sistem21_ventaboletos;password=3Otr^53b4", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.20-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Asiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asiento");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Columna).HasColumnType("int(11)");
            entity.Property(e => e.Fila).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Boleto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("boleto");

            entity.HasIndex(e => e.IdHorario, "FK_Boleto_Horario");

            entity.HasIndex(e => e.IdSala, "FK_Boleto_Sala");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdHorario)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Horario");
            entity.Property(e => e.IdSala)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Sala");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Boleto)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boleto_Horario");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Boleto)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Boleto_Sala");
        });

        modelBuilder.Entity<Clasificacion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clasificacion");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("genero");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(45);
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("horario");

            entity.HasIndex(e => e.IdSala, "FK_Horario_Sala");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("Hora_Inicio");
            entity.Property(e => e.HoraTerminacion)
                .HasColumnType("time")
                .HasColumnName("Hora_Terminacion");
            entity.Property(e => e.IdPelicula)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Pelicula");
            entity.Property(e => e.IdSala)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Sala");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Horario)
                .HasForeignKey(d => d.IdSala)
                .HasConstraintName("FK_Horario_Sala");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula");

            entity.HasIndex(e => e.IdClasificacion, "Fk_Pelicula_Clasificacion");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Año).HasColumnType("int(11)");
            entity.Property(e => e.Duracion).HasColumnType("time");
            entity.Property(e => e.IdClasificacion)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Clasificacion");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasPrecision(10);
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

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdGenero)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Genero");
            entity.Property(e => e.IdPelicula)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Pelicula");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.PeliculaGenero)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_genero_Genero");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculaGenero)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_genero_Pelicula");
        });

        modelBuilder.Entity<PeliculaHorario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pelicula_horario");

            entity.HasIndex(e => e.IdPelicula, "pelicula_peliculahorario_Pelicula");

            entity.HasIndex(e => e.IdHorario, "pelicula_peliculahorario_horario");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdHorario)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Horario");
            entity.Property(e => e.IdPelicula)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Pelicula");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.PeliculaHorario)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_peliculahorario_horario");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculaHorario)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pelicula_peliculahorario_Pelicula");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(13);
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala");

            entity.HasIndex(e => e.IdSalaAsiento, "FK_Sala_SalaAsiento_idx");

            entity.HasIndex(e => e.IdTipoPantalla, "FK_Sala_TipoPantalla_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Columnas).HasColumnType("int(11)");
            entity.Property(e => e.Filas).HasColumnType("int(11)");
            entity.Property(e => e.IdSalaAsiento).HasColumnType("int(11)");
            entity.Property(e => e.IdTipoPantalla).HasColumnType("int(11)");

            entity.HasOne(d => d.IdSalaAsientoNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.IdSalaAsiento)
                .HasConstraintName("FK_Sala_SalaAsiento");

            entity.HasOne(d => d.IdTipoPantallaNavigation).WithMany(p => p.Sala)
                .HasForeignKey(d => d.IdTipoPantalla)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sala_TipoPantalla");
        });

        modelBuilder.Entity<SalaAsiento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sala_asiento");

            entity.HasIndex(e => e.IdAsiento, "Id_Asiento");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdAsiento)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Asiento");
            entity.Property(e => e.IdSala)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Sala");

            entity.HasOne(d => d.IdAsientoNavigation).WithMany(p => p.SalaAsiento)
                .HasForeignKey(d => d.IdAsiento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sala_asiento_ibfk_1");
        });

        modelBuilder.Entity<Tipopantalla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipopantalla");

            entity.HasIndex(e => e.Nombre, "Nombre").IsUnique();

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(5);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Username, "Username").IsUnique();

            entity.HasIndex(e => e.IdBoleto, "fk_Usuario_Boleto");

            entity.HasIndex(e => e.IdRol, "fk_Usuario_Rol");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contraseña).HasMaxLength(128);
            entity.Property(e => e.IdBoleto)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Boleto");
            entity.Property(e => e.IdRol)
                .HasColumnType("int(11)")
                .HasColumnName("Id_Rol");
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
