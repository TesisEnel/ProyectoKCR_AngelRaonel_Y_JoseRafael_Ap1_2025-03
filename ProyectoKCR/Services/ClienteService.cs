using Microsoft.EntityFrameworkCore;
using ProyectoKCR.DAL;
using ProyectoKCR.Models;
using System.Linq.Expressions;

namespace ProyectoKCR.Services;

public class ClienteService(IDbContextFactory<Contexto> DbFactory)
{
    public async Task<bool> Existe(int Idcliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.clientes
            .AnyAsync(j => j.IdCliente == Idcliente);
    }

    public async Task<bool> Insertar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.clientes.Add(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Modificar(Clientes cliente)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.clientes.Update(cliente);
        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Clientes cliente)
    {
        if (!await Existe(cliente.IdCliente))
        {
            return await Insertar(cliente);
        }
        else
        {
            return await Modificar(cliente);
        }
    }

    public async Task<Clientes> Buscar(int clienteId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.clientes.FirstOrDefaultAsync(c => c.IdCliente == clienteId);
    }

    public async Task<List<Clientes>> Listar(Expression<Func<Clientes, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.clientes
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }

}
