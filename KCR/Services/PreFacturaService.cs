using KCR.Data;
using KCR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        return await contexto.preFacturas.Include(p => p.PreFacturaDetalles).Where(criterio).AsNoTracking().ToListAsync();
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
}

public enum TipoOperacion
{
    Suma = 1,
    Resta = 2
}
