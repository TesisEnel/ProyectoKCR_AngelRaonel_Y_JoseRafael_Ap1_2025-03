using KCR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KCR.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Empleados> empleados { get; set; }
    public DbSet<Clientes> clientes { get; set; }
    public DbSet<Materiales> materiales { get; set; }
    public DbSet<PreFacturas> preFacturas { get; set; }
    public DbSet<Turnos> turnos { get; set; }
    public DbSet<PreFacturaDetalles> preFacturaDetalles { get; set; }
    public DbSet<Servicios> servicios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Materiales>
            (entity =>
            {
                entity.HasData(
                    new Materiales { IdMaterial = 1, Nombre = "Papel Bond 8.5x11", Existencia = 500, PrecioUnitario = 1 },
                    new Materiales { IdMaterial = 2, Nombre = "Papel Bond 8.5x14", Existencia = 500, PrecioUnitario = 1.5 },
                    new Materiales { IdMaterial = 3, Nombre = "Papel Bond 11x17", Existencia = 500, PrecioUnitario = 2 },
                    new Materiales { IdMaterial = 4, Nombre = "Cartonite 11x17", Existencia = 500, PrecioUnitario = 10 },
                    new Materiales { IdMaterial = 5, Nombre = "Opalina 11x17", Existencia = 500, PrecioUnitario = 15 }
                );
            });

        builder.Entity<Servicios>(entity =>
        {
            entity.HasData(
                // COPIAS (Precios con M para decimal)
                new Servicios { IdServicio = 1, Nombre = "COPIA B/N 8.5x11 (Bond)", Precio = 5.00 },
                new Servicios { IdServicio = 2, Nombre = "COPIA B/N 8.5x14 (Bond)", Precio = 10.00 },
                new Servicios { IdServicio = 3, Nombre = "COPIA B/N 11x17 (Bond)", Precio = 15.00 },
                new Servicios { IdServicio = 4, Nombre = "COPIA COLOR 8.5x11 (Bond)", Precio = 15.00 },

                // IMPRESIONES
                new Servicios { IdServicio = 5, Nombre = "IMPRESION B/N 8.5x11 (Bond)", Precio = 5.00 },
                new Servicios { IdServicio = 6, Nombre = "IMPRESION COLOR 8.5x11 (Bond)", Precio = 20.00 },
                new Servicios { IdServicio = 7, Nombre = "IMPRESION COLOR 8.5x14 (Bond)", Precio = 25.00 },
                new Servicios { IdServicio = 8, Nombre = "IMPRESION COLOR 11x17 (Bond)", Precio = 40.00 },
                new Servicios { IdServicio = 9, Nombre = "IMPRESION COLOR 11x17 (Cartonité)", Precio = 75.00 },
                new Servicios { IdServicio = 10, Nombre = "IMPRESION COLOR 11x17 (Opalina)", Precio = 85.00 },

                // Planos
                new Servicios { IdServicio = 11, Nombre = "IMPRESION PLANO 24x36", Precio = 50.00 },
                new Servicios { IdServicio = 12, Nombre = "IMPRESION PLANO 18x24", Precio = 30.00 },

                // ENCUADERNADO
                new Servicios { IdServicio = 13, Nombre = "ENCUADERNADO (Pequeño/Carta)", Precio = 50.00 },
                new Servicios { IdServicio = 14, Nombre = "ENCUADERNADO (Mediano/Oficio)", Precio = 75.00 },
                new Servicios { IdServicio = 15, Nombre = "ENCUADERNADO (Grande/Doble Carta)", Precio = 100.00 },

                // OTROS SERVICIOS
                new Servicios { IdServicio = 16, Nombre = "ESCANER", Precio = 15.00 },
                new Servicios { IdServicio = 17, Nombre = "DISEÑO", Precio = 500.00 }
            );
        });


        builder.Entity<PreFacturaDetalles>()
            .HasOne(pd => pd.Materiales)
            .WithMany(m => m.PreFacturaDetalles)
            .HasForeignKey(pd => pd.IdMaterial)
            .IsRequired(false);


        builder.Entity<PreFacturaDetalles>()
            .HasOne(pd => pd.Servicios)
            .WithMany(s => s.PreFacturaDetalles)
            .HasForeignKey(pd => pd.IdServicio)
            .IsRequired(false);
    } 
}