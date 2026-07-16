using ControleGastos.API.Data;
using ControleGastos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Repositories;

public class PessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pessoa> AdicionarAsync(Pessoa pessoa, CancellationToken cancellationToken = default)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync(cancellationToken);
        return pessoa;
    }

    public async Task<List<Pessoa>> ListarAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas.AsNoTracking().OrderBy(p => p.Id).ToListAsync(cancellationToken);
    }

    public async Task<Pessoa?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Pessoa?> ObterPorIdSomenteLeituraAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Pessoas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task RemoverAsync(Pessoa pessoa, CancellationToken cancellationToken = default)
    {
        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
