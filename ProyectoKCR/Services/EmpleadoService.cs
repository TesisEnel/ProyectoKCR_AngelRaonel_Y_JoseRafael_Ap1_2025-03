using Microsoft.EntityFrameworkCore;
using ProyectoKCR.DAL;
using ProyectoKCR.Models;
using System.Linq.Expressions;

namespace ProyectoKCR.Services;

public class EmpleadoService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int idempleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.empleados
            .AnyAsync(e => e.IdEmpleado == idempleado);
    }

    public async Task<bool> Insertar(Empleados empleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.empleados.Add(empleado);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Empleados empleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.empleados.Update(empleado);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Empleados empleado)
    {
        if (!await Existe(empleado.IdEmpleado))
        {
            return await Insertar(empleado);
        }
        else
        {
            return await Modificar(empleado);
        }
    }

    public async Task<Empleados> Buscar(int idempleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idempleado);
    }

    public async Task<List<Empleados>> Listar(Expression<Func<Empleados, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.empleados
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
