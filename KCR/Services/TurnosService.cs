using KCR.Data;
using KCR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KCR.Services;

public class TurnoService(IDbContextFactory<ApplicationDbContext> DbFactory)
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

    public async Task<Turnos?> Buscar(int idturnos)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.turnos
            .Include(t => t.Clientes)
            .Include(t => t.Servicios)
            .FirstOrDefaultAsync(t => t.IdTurno == idturnos);
    }

    public async Task<List<Turnos>> Listar(Expression<Func<Turnos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.turnos
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<bool> CambiarEstadoAsync(int turnoId, string nuevoEstado)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var turno = await contexto.turnos.FindAsync(turnoId);
        if (turno == null)
        {
            return false;
        }
        turno.Estado = nuevoEstado;
        return await contexto.SaveChangesAsync() > 0;
    }
}

