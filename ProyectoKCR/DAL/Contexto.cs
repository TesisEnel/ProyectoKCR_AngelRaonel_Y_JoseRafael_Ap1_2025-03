using Microsoft.EntityFrameworkCore;
using ProyectoKCR.Models;

namespace ProyectoKCR.DAL;

public class Contexto : DbContext
{
    public Contexto(DbContextOptions<Contexto> options) : base(options) { }
    public DbSet<Empleados> empleados { get; set; }
    public DbSet<Clientes> clientes { get; set; }
    public DbSet<Materiales> materiales { get; set; }
    public DbSet<PreFacturas> preFacturas { get; set; }
    public DbSet<Turnos> turnos { get; set; }
    public DbSet<DetallePreFactura> detallePreFactura { get; set; }
    public DbSet<Servicios> servicios { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Materiales>(entity =>
        {
            entity.HasData(
                new Materiales
                {
                    IdMaterial = 1,
                    Nombre = "Papel",
                    PrecioUnitario = 5,
                    Existencia = 100
                }
            );
        });

      

    }

}
