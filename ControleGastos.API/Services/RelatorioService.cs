using ControleGastos.API.DTOs;
using ControleGastos.API.Repositories;
using ControleGastos.API.Models;

namespace ControleGastos.API.Services;

public class RelatorioService
{
    private readonly PessoaRepository _pessoaRepository;
    private readonly TransacaoRepository _transacaoRepository;

    public RelatorioService(PessoaRepository pessoaRepository, TransacaoRepository transacaoRepository)
    {
        _pessoaRepository = pessoaRepository;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<RelatorioTotalDto> ObterTotaisAsync(CancellationToken cancellationToken = default)
    {
        var pessoas = await _pessoaRepository.ListarAsync(cancellationToken);
        var transacoes = await _transacaoRepository.ListarAsync(cancellationToken);
        var transacoesPorPessoa = transacoes.ToLookup(t => t.PessoaId);
        var relatorio = new RelatorioTotalDto();

        foreach (var pessoa in pessoas)
        {
            var transacoesDaPessoa = transacoesPorPessoa[pessoa.Id];
            var totalReceitas = transacoesDaPessoa.Where(t => t.Tipo == TipoTransacao.Receita).Sum(t => t.Valor);
            var totalDespesas = transacoesDaPessoa.Where(t => t.Tipo == TipoTransacao.Despesa).Sum(t => t.Valor);
            var saldo = totalReceitas - totalDespesas;

            relatorio.Pessoas.Add(new RelatorioPessoaDto
            {
                PessoaId = pessoa.Id,
                Nome = pessoa.Nome,
                TotalReceitas = totalReceitas,
                TotalDespesas = totalDespesas,
                Saldo = saldo
            });

            relatorio.TotalReceitasGeral += totalReceitas;
            relatorio.TotalDespesasGeral += totalDespesas;
        }

        relatorio.SaldoGeral = relatorio.TotalReceitasGeral - relatorio.TotalDespesasGeral;
        return relatorio;
    }
}
