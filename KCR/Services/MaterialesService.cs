using KCR.Data;
using KCR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KCR.Services;

public class MaterialesService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<bool> Existe(int idmaterial)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.materiales.AnyAsync(m => m.IdMaterial == idmaterial);
    }

    public async Task<bool> Insertar(Materiales material)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.materiales.Add(material);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Materiales material)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.materiales.Update(material);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Materiales material)
    {
        if (!await Existe(material.IdMaterial))
            return await Insertar(material);
        else
            return await Modificar(material);
    }

    public async Task<bool> Eliminar(int idmaterial)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.materiales.Where(m => m.IdMaterial == idmaterial).AsNoTracking().ExecuteDeleteAsync() > 0;
    }

    public async Task<Materiales?> Buscar(int idmaterial)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.materiales.FirstOrDefaultAsync(m => m.IdMaterial == idmaterial);
    }

    public async Task<List<Materiales>> Listar(Expression<Func<Materiales, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.materiales.Where(criterio).AsNoTracking().ToListAsync();
    }
}
