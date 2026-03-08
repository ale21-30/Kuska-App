using Kuska.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Kuska.Data;

public class KuskaDbContext : DbContext
{

    public DbSet<ListaUtil> ListaUtiles { get; set; }
    public DbSet<Apadrinamiento> Apadrinamientos { get; set; }
    public KuskaDbContext(DbContextOptions<KuskaDbContext> options) : base(options) { }

    public DbSet<Rol> Roles { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Nina> Ninas { get; set; }
    public DbSet<Fondo> Fondos { get; set; }
    public DbSet<AporteFondo> AportesFondo { get; set; }
    public DbSet<Mentora> Mentoras { get; set; }
    public DbSet<MentoraHabilidad> MentoraHabilidades { get; set; }
    public DbSet<Sesion> Sesiones { get; set; }
    public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }
    public DbSet<Reporte> Reportes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- PRIMARY KEYS EXPLÍCITAS ----
        modelBuilder.Entity<Rol>().HasKey(r => r.RolId);
        modelBuilder.Entity<Usuario>().HasKey(u => u.UsuarioId);
        modelBuilder.Entity<Nina>().HasKey(n => n.NinaId);
        modelBuilder.Entity<Fondo>().HasKey(f => f.FondoId);
        modelBuilder.Entity<AporteFondo>().HasKey(a => a.AporteId);
        modelBuilder.Entity<Mentora>().HasKey(m => m.MentoraId);
        modelBuilder.Entity<MentoraHabilidad>().HasKey(m => m.HabilidadId);
        modelBuilder.Entity<Sesion>().HasKey(s => s.SesionId);
        modelBuilder.Entity<AuditoriaLog>().HasKey(a => a.LogId);
        modelBuilder.Entity<Reporte>().HasKey(r => r.ReporteId);

        // ---- NOMBRES DE TABLAS EXACTOS ----
        modelBuilder.Entity<Rol>().ToTable("Roles");
        modelBuilder.Entity<Usuario>().ToTable("Usuarios");
        modelBuilder.Entity<Nina>().ToTable("Ninas");
        modelBuilder.Entity<Fondo>().ToTable("Fondos");
        modelBuilder.Entity<AporteFondo>().ToTable("AportesFondo");
        modelBuilder.Entity<Mentora>().ToTable("Mentoras");
        modelBuilder.Entity<MentoraHabilidad>().ToTable("MentoraHabilidades");
        modelBuilder.Entity<Sesion>().ToTable("Sesiones");
        modelBuilder.Entity<AuditoriaLog>().ToTable("AuditoriaLog");
        modelBuilder.Entity<Reporte>().ToTable("Reportes");

        // ---- USUARIO ----
        modelBuilder.Entity<Usuario>(e => {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.PasswordHash).HasMaxLength(500);
            e.HasOne(u => u.Rol)
             .WithMany(r => r.Usuarios)
             .HasForeignKey(u => u.RolId);
        });

        // ---- NINA ----
        modelBuilder.Entity<Nina>(e => {
            e.Property(n => n.NombrePila).HasMaxLength(50);
            e.HasOne(n => n.Madre)
             .WithMany(u => u.Ninas)
             .HasForeignKey(n => n.MadreId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- FONDO ----
        modelBuilder.Entity<Fondo>(e => {
            e.Property(f => f.MetaMonto).HasColumnType("decimal(10,2)");
            e.Property(f => f.MontoActual).HasColumnType("decimal(10,2)");
        });

        // ---- APORTE FONDO ----
        modelBuilder.Entity<AporteFondo>(e => {
            e.Property(a => a.Monto).HasColumnType("decimal(10,2)");
            e.HasOne(a => a.Fondo)
             .WithMany(f => f.Aportes)
             .HasForeignKey(a => a.FondoId);
            e.HasOne(a => a.Usuario)
             .WithMany()
             .HasForeignKey(a => a.UsuarioId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- MENTORA ----
        modelBuilder.Entity<Mentora>(e => {
            e.Property(m => m.Rating).HasColumnType("decimal(3,2)");
            e.HasOne(m => m.Usuario)
             .WithOne(u => u.Mentora)
             .HasForeignKey<Mentora>(m => m.UsuarioId);
        });

        // ---- SESION ----
        modelBuilder.Entity<Sesion>(e => {
            e.HasOne(s => s.Mentora)
             .WithMany(m => m.Sesiones)
             .HasForeignKey(s => s.MentoraId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Nina)
             .WithMany(n => n.Sesiones)
             .HasForeignKey(s => s.NinaId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(s => s.Supervisora)
             .WithMany()
             .HasForeignKey(s => s.SupervisoraId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- AUDITORIA ----
        modelBuilder.Entity<AuditoriaLog>(e => {
            e.HasOne(a => a.Usuario)
             .WithMany()
             .HasForeignKey(a => a.UsuarioId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ---- REPORTE ----
        modelBuilder.Entity<Reporte>(e => {
            e.HasOne(r => r.Sesion)
             .WithMany(s => s.Reportes)
             .HasForeignKey(r => r.SesionId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(r => r.Usuario)
             .WithMany()
             .HasForeignKey(r => r.ReportadoPor)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---- SEED: ROLES ----
        modelBuilder.Entity<Rol>().HasData(
            new Rol { RolId = 1, Nombre = "Admin", Descripcion = "Administrador de la plataforma" },
            new Rol { RolId = 2, Nombre = "Madre", Descripcion = "Madre o tutora de una niña beneficiaria" },
            new Rol { RolId = 3, Nombre = "Mentora", Descripcion = "Profesional verificada que ofrece mentorías" },
            new Rol { RolId = 4, Nombre = "Empresa", Descripcion = "Empresa patrocinadora RSE" }
        );

        modelBuilder.Entity<ListaUtil>().HasKey(x => x.ItemId);
        modelBuilder.Entity<ListaUtil>().ToTable("ListaUtiles");
        modelBuilder.Entity<ListaUtil>()
            .HasOne(x => x.Nina).WithMany(n => n.ListaUtiles)
            .HasForeignKey(x => x.NinaId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ListaUtil>()
            .HasOne(x => x.Apadrinador).WithMany()
            .HasForeignKey(x => x.ApadrinadoPor).OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Apadrinamiento>().HasKey(x => x.ApadrinamientoId);
        modelBuilder.Entity<Apadrinamiento>().ToTable("Apadrinamientos");
        modelBuilder.Entity<Apadrinamiento>()
            .HasOne(x => x.Nina).WithMany(n => n.Apadrinamientos)
            .HasForeignKey(x => x.NinaId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Apadrinamiento>()
            .HasOne(x => x.Empresa).WithMany()
            .HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.NoAction);

    }
}