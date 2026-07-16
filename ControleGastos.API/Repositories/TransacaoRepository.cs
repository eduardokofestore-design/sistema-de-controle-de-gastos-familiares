using ControleGastos.API.Data;
using ControleGastos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleGastos.API.Repositories;

public class TransacaoRepository
{
    private readonly AppDbContext _context;

    public TransacaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Transacao> AdicionarAsync(Transacao transacao, CancellationToken cancellationToken = default)
    {
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync(cancellationToken);
        return transacao;
    }

    public async Task<List<Transacao>> ListarAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .OrderBy(t => t.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Transacao>> ListarPorPessoaAsync(int pessoaId, CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes.AsNoTracking().Where(t => t.PessoaId == pessoaId).ToListAsync(cancellationToken);
    }

    public async Task<Transacao?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Transacoes
            .AsNoTracking()
            .Include(t => t.Pessoa)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
