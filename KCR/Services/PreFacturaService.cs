using KCR.Data;
using KCR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static KCR.Components.EmpleadoPages.PreFacturaEm;

namespace KCR.Services;

public class PreFacturaService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.preFacturas.AnyAsync(p => p.IdPreFactura == id);
    }

    public async Task AfectarExistencia(PreFacturaDetalles[] detalle, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var item in detalle)
        {
            // --- AGREGA ESTE BLOQUE ---
            // Si no hay ID de Material (es null o 0), es un servicio.
            // No hacemos nada y pasamos al siguiente.
            if (item.IdMaterial == null || item.IdMaterial <= 0)
            {
                continue;
            }
            // --------------------------

            var material = await contexto.materiales.SingleAsync(m => m.IdMaterial == item.IdMaterial);

            if (tipoOperacion == TipoOperacion.Suma)
                material.Existencia += item.Cantidad;
            else
                material.Existencia -= item.Cantidad;

            await contexto.SaveChangesAsync();
        }
    }

    public async Task<bool> Insertar(PreFacturas preFactura)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.preFacturas.Add(preFactura);
        await AfectarExistencia(preFactura.PreFacturaDetalles.ToArray(), TipoOperacion.Resta);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(PreFacturas preFactura)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var detallesOriginales = await contexto.preFacturaDetalles
            .Where(d => d.IdPreFactura == preFactura.IdPreFactura)
            .AsNoTracking()
            .ToListAsync();

        await AfectarExistencia(detallesOriginales.ToArray(), TipoOperacion.Suma);
        contexto.preFacturaDetalles.RemoveRange(detallesOriginales);
        contexto.preFacturas.Update(preFactura);
        await AfectarExistencia(preFactura.PreFacturaDetalles.ToArray(), TipoOperacion.Resta);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(PreFacturas preFactura)
    {
        if (!await Existe(preFactura.IdPreFactura))
            return await Insertar(preFactura);
        else
            return await Modificar(preFactura);
    }

    public async Task<bool> Eliminar(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var preFactura = await Buscar(id);

        await AfectarExistencia(preFactura.PreFacturaDetalles.ToArray(), TipoOperacion.Resta);
        contexto.preFacturaDetalles.RemoveRange(preFactura.PreFacturaDetalles);
        contexto.preFacturas.Remove(preFactura);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<PreFacturas?> Buscar(int Id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.preFacturas.Include(p => p.PreFacturaDetalles).FirstOrDefaultAsync(p => p.IdPreFactura == Id);
    }

    public async Task<List<PreFacturas>> Listar(Expression<Func<PreFacturas, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.preFacturas
            .Include(p => p.PreFacturaDetalles)
            .Include(p => p.Clientes) 
            .Include(p => p.Empleado)  
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Servicios>> ListarServicios()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.servicios
            .Where(s => s.IdServicio > 0)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Materiales>> ListarMateriales()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.materiales.Where(m => m.IdMaterial > 0).AsNoTracking().ToListAsync();
    }

    public async Task<List<PreFacturaDetalles>> BuscarInventario(string query)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<PreFacturaDetalles>();
        }

        var lowerQuery = query.ToLower();

        var servicios = await contexto.servicios
            .Where(s => s.Nombre.ToLower().Contains(lowerQuery) ||
                (s.Tipo != null && s.Tipo.ToLower().Contains(lowerQuery)))
            .Select(s => new PreFacturaDetalles
            {
                IdServicio = s.IdServicio,
                IdMaterial = null,
                PrecioUnitario = (decimal)s.Precio,
                Cantidad = 1,
                Servicios = new Servicios { Nombre = s.Nombre, Tipo = s.Tipo ?? "Servicio" }
            })
            .ToListAsync();

        var materiales = await contexto.materiales
            .Where(m => m.Existencia > 0)
            .Where(m => m.Nombre.ToLower().Contains(lowerQuery))
            .Select(m => new PreFacturaDetalles
            {
                IdMaterial = m.IdMaterial,
                IdServicio = null,
                PrecioUnitario = (decimal)m.PrecioUnitario,
                Cantidad = 1,
                Materiales = new Materiales { Nombre = m.Nombre }
            })
            .ToListAsync();

        return servicios.Concat(materiales)
            .OrderBy(i => i.Servicios?.Nombre ?? i.Materiales?.Nombre)
            .ToList();
    }
}

public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}
