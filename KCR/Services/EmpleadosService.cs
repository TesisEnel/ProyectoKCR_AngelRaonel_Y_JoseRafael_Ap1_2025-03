using KCR.Data;
using KCR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KCR.Services;

public class EmpleadoService(IDbContextFactory<ApplicationDbContext> DbFactory)
{
    public async Task<List<Empleados>> ListarTodosAsync()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.empleados
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<bool> Eliminar(int idempleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var empleado = await contexto.empleados.FindAsync(idempleado);

        if (empleado == null)
            return false;
        contexto.empleados.Remove(empleado);
        return await contexto.SaveChangesAsync() > 0;
    }
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
        if (empleado.IdEmpleado == 0 || !await Existe(empleado.IdEmpleado))
        {
            return await Insertar(empleado);
        }
        else
        {
            return await Modificar(empleado);
        }
    }

    public async Task<bool> Actualizar(Empleados empleado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.empleados.Update(empleado);
        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<Empleados?> Buscar(int? idempleado)
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
