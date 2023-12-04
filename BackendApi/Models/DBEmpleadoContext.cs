using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackendApi.Models
{
    public partial class DBEmpleadoContext : DbContext
    {
        public DBEmpleadoContext()
        {
        }

        public DBEmpleadoContext(DbContextOptions<DBEmpleadoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departamento> Departamentos { get; set; } = null!;
        public virtual DbSet<Empleado> Empleados { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(e => e.IdDepartamento)
                    .HasName("PK__Departam__787A433DA5A04EFE");

                entity.ToTable("Departamento");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Empleado>(entity =>
            {
                entity.HasKey(e => e.IdEmpleado)
                    .HasName("PK__Empleado__CE6D8B9EC00ED102");

                entity.ToTable("Empleado");

                entity.Property(e => e.FechaContrato)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombreCompleto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdDepartamentoNavigation)
                    .WithMany(p => p.Empleados)
                    .HasForeignKey(d => d.IdDepartamento)
                    .HasConstraintName("FK__Empleado__IdDepa__4CA06362");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
