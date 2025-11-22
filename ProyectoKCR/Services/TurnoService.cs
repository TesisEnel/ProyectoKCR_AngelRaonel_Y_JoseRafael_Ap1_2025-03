using Microsoft.EntityFrameworkCore;
using ProyectoKCR.DAL;
using ProyectoKCR.Models;
using System.Linq.Expressions;

namespace ProyectoKCR.Services;

public class TurnoService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int idturnos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.turnos.AnyAsync(t => t.IdTurno == idturnos);
    }

    public async Task<bool> Insertar(Turnos turno)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.turnos.Add(turno);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Turnos turnos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.turnos.Update(turnos);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Turnos turnos)
    {
        if (!await Existe(turnos.IdTurno))
        {
            return await Insertar(turnos);
        }
        else
        {
            return await Modificar(turnos);
        }
    }

    public async Task<Turnos> Buscar(int idturnos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.turnos.FirstOrDefaultAsync(t => t.IdTurno == idturnos);
    }

    public async Task<List<Turnos>> Listar(Expression<Func<Turnos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.turnos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
}
