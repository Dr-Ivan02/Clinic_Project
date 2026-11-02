using System;
using System.Collections.Generic;
using ClinicaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClinicaAPI.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Antecedente> Antecedentes { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<ConsultaMedica> ConsultaMedicas { get; set; }

    public virtual DbSet<Diagnostico> Diagnosticos { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<MedicoEspecialidad> MedicoEspecialidads { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Antecedente>(entity =>
        {
            entity.ToTable("Antecedente");

            entity.HasIndex(e => e.ExpedienteId, "IX_Antecedente_Expediente");

            entity.Property(e => e.AntecedenteId).HasColumnName("AntecedenteID");
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.ExpedienteId).HasColumnName("ExpedienteID");
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.RegistradoPorUsuarioId).HasColumnName("RegistradoPorUsuarioID");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Expediente).WithMany(p => p.Antecedentes)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Antecedente_Expediente");

            entity.HasOne(d => d.RegistradoPorUsuario).WithMany(p => p.Antecedentes)
                .HasForeignKey(d => d.RegistradoPorUsuarioId)
                .HasConstraintName("FK_Antecedente_Usuario");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.CitaId);

            entity.HasIndex(e => new { e.MedicoId, e.FechaHoraInicio, e.FechaHoraFin }, "IX_Cita_Medico_Horario");

            entity.HasIndex(e => new { e.PacienteId, e.FechaHoraInicio }, "IX_Cita_Paciente_Fecha").IsDescending(false, true);

            entity.Property(e => e.CitaId).HasColumnName("CitaID");
            entity.Property(e => e.CreadaPorUsuarioId).HasColumnName("CreadaPorUsuarioID");
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Programada");
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.FechaHoraFin).HasPrecision(0);
            entity.Property(e => e.FechaHoraInicio).HasPrecision(0);
            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.Motivo).HasMaxLength(500);
            entity.Property(e => e.PacienteId).HasColumnName("PacienteID");

            entity.HasOne(d => d.CreadaPorUsuario).WithMany(p => p.Cita)
                .HasForeignKey(d => d.CreadaPorUsuarioId)
                .HasConstraintName("FK_Cita_Creador");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Cita)
                .HasForeignKey(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Paciente");

            entity.HasOne(d => d.MedicoEspecialidad).WithMany(p => p.Cita)
                .HasForeignKey(d => new { d.MedicoId, d.EspecialidadId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_MedicoEspecialidad");
        });

        modelBuilder.Entity<ConsultaMedica>(entity =>
        {
            entity.HasKey(e => e.ConsultaId);

            entity.ToTable("ConsultaMedica");

            entity.HasIndex(e => new { e.ExpedienteId, e.FechaHora }, "IX_Consulta_Expediente_Fecha").IsDescending(false, true);

            entity.HasIndex(e => e.CitaId, "UX_ConsultaMedica_Cita")
                .IsUnique()
                .HasFilter("([CitaID] IS NOT NULL)");

            entity.Property(e => e.ConsultaId).HasColumnName("ConsultaID");
            entity.Property(e => e.CitaId).HasColumnName("CitaID");
            entity.Property(e => e.ExpedienteId).HasColumnName("ExpedienteID");
            entity.Property(e => e.FechaHora)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.MotivoConsulta).HasMaxLength(500);
            entity.Property(e => e.PlanSeguimiento).HasMaxLength(1000);
            entity.Property(e => e.RegistradaPorUsuarioId).HasColumnName("RegistradaPorUsuarioID");

            entity.HasOne(d => d.Cita).WithOne(p => p.ConsultaMedica)
                .HasForeignKey<ConsultaMedica>(d => d.CitaId)
                .HasConstraintName("FK_ConsultaMedica_Cita");

            entity.HasOne(d => d.Expediente).WithMany(p => p.ConsultaMedicas)
                .HasForeignKey(d => d.ExpedienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsultaMedica_Expediente");

            entity.HasOne(d => d.Medico).WithMany(p => p.ConsultaMedicas)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ConsultaMedica_Medico");

            entity.HasOne(d => d.RegistradaPorUsuario).WithMany(p => p.ConsultaMedicas)
                .HasForeignKey(d => d.RegistradaPorUsuarioId)
                .HasConstraintName("FK_ConsultaMedica_Usuario");
        });

        modelBuilder.Entity<Diagnostico>(entity =>
        {
            entity.ToTable("Diagnostico");

            entity.HasIndex(e => e.ConsultaId, "IX_Diagnostico_Consulta");

            entity.Property(e => e.DiagnosticoId).HasColumnName("DiagnosticoID");
            entity.Property(e => e.Codigo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ConsultaId).HasColumnName("ConsultaID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Tipo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Principal");

            entity.HasOne(d => d.Consulta).WithMany(p => p.Diagnosticos)
                .HasForeignKey(d => d.ConsultaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Diagnostico_Consulta");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.ToTable("Especialidad");

            entity.HasIndex(e => e.Nombre, "UQ_Especialidad_Nombre").IsUnique();

            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(300);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.ToTable("Expediente");

            entity.HasIndex(e => e.PacienteId, "UQ_Expediente_Paciente").IsUnique();

            entity.Property(e => e.ExpedienteId).HasColumnName("ExpedienteID");
            entity.Property(e => e.FechaApertura)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ObservacionesGenerales).HasMaxLength(1000);
            entity.Property(e => e.PacienteId).HasColumnName("PacienteID");

            entity.HasOne(d => d.Paciente).WithOne(p => p.Expediente)
                .HasForeignKey<Expediente>(d => d.PacienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expediente_Paciente");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.ToTable("Factura");

            entity.HasIndex(e => e.ConsultaId, "UQ_Factura_Consulta").IsUnique();

            entity.HasIndex(e => e.NumeroFactura, "UQ_Factura_Numero").IsUnique();

            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.ConsultaId).HasColumnName("ConsultaID");
            entity.Property(e => e.CreadaPorUsuarioId).HasColumnName("CreadaPorUsuarioID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaEmision)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Impuestos).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.NumeroFactura)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Subtotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Total)
                .HasComputedColumnSql("([Subtotal]+[Impuestos])", true)
                .HasColumnType("decimal(13, 2)");

            entity.HasOne(d => d.Consulta).WithOne(p => p.Factura)
                .HasForeignKey<Factura>(d => d.ConsultaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Factura_Consulta");

            entity.HasOne(d => d.CreadaPorUsuario).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.CreadaPorUsuarioId)
                .HasConstraintName("FK_Factura_Usuario");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.ToTable("Medico");

            entity.HasIndex(e => e.NumeroExequatur, "UQ_Medico_NumeroExequatur").IsUnique();

            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(80);
            entity.Property(e => e.Correo)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.Nombres).HasMaxLength(80);
            entity.Property(e => e.NumeroExequatur)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MedicoEspecialidad>(entity =>
        {
            entity.HasKey(e => new { e.MedicoId, e.EspecialidadId });

            entity.ToTable("MedicoEspecialidad");

            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.EspecialidadId).HasColumnName("EspecialidadID");
            entity.Property(e => e.FechaAsignacion).HasDefaultValueSql("(CONVERT([date],sysdatetime()))");

            entity.HasOne(d => d.Especialidad).WithMany(p => p.MedicoEspecialidads)
                .HasForeignKey(d => d.EspecialidadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicoEspecialidad_Especialidad");

            entity.HasOne(d => d.Medico).WithMany(p => p.MedicoEspecialidads)
                .HasForeignKey(d => d.MedicoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicoEspecialidad_Medico");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("Paciente");

            entity.HasIndex(e => new { e.Apellidos, e.Nombres }, "IX_Paciente_Apellidos_Nombres");

            entity.HasIndex(e => e.Cedula, "UQ_Paciente_Cedula").IsUnique();

            entity.Property(e => e.PacienteId).HasColumnName("PacienteID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(80);
            entity.Property(e => e.Cedula)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.FechaRegistro)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Nombres).HasMaxLength(80);
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.ToTable("Pago");

            entity.HasIndex(e => new { e.FacturaId, e.FechaPago }, "IX_Pago_Factura_Fecha").IsDescending(false, true);

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.FacturaId).HasColumnName("FacturaID");
            entity.Property(e => e.FechaPago)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Monto).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Referencia)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistradoPorUsuarioId).HasColumnName("RegistradoPorUsuarioID");

            entity.HasOne(d => d.Factura).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Factura");

            entity.HasOne(d => d.RegistradoPorUsuario).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.RegistradoPorUsuarioId)
                .HasConstraintName("FK_Pago_Usuario");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol");

            entity.HasIndex(e => e.Nombre, "UQ_Rol_Nombre").IsUnique();

            entity.Property(e => e.RolId)
                .ValueGeneratedOnAdd()
                .HasColumnName("RolID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.ToTable("Tratamiento");

            entity.HasIndex(e => e.ConsultaId, "IX_Tratamiento_Consulta");

            entity.Property(e => e.TratamientoId).HasColumnName("TratamientoID");
            entity.Property(e => e.ConsultaId).HasColumnName("ConsultaID");
            entity.Property(e => e.Descripcion).HasMaxLength(500);
            entity.Property(e => e.Instrucciones).HasMaxLength(1000);

            entity.HasOne(d => d.Consulta).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.ConsultaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tratamiento_Consulta");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ_Usuario_NombreUsuario").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Correo)
                .HasMaxLength(254)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.HashContrasena)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.MedicoId).HasColumnName("MedicoID");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("RolID");

            entity.HasOne(d => d.Medico).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.MedicoId)
                .HasConstraintName("FK_Usuario_Medico");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
