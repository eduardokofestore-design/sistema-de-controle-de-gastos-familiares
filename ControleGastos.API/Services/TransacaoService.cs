using ControleGastos.API.DTOs;
using ControleGastos.API.Models;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services;

public class TransacaoService
{
    private readonly TransacaoRepository _transacaoRepository;
    private readonly PessoaRepository _pessoaRepository;

    public TransacaoService(TransacaoRepository transacaoRepository, PessoaRepository pessoaRepository)
    {
        _transacaoRepository = transacaoRepository;
        _pessoaRepository = pessoaRepository;
    }

    public async Task<(bool Sucesso, string? MensagemErro, TransacaoResponseDto? Resultado)> CriarAsync(TransacaoCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Descricao))
        {
            return (false, "A descrição é obrigatória.", null);
        }

        if (dto.Valor <= 0)
        {
            return (false, "O valor deve ser maior que zero.", null);
        }

        var pessoa = await _pessoaRepository.ObterPorIdAsync(dto.PessoaId, cancellationToken);
        if (pessoa is null)
        {
            return (false, "A pessoa informada não existe.", null);
        }

        if (pessoa.Idade < 18 && dto.Tipo == TipoTransacao.Receita)
        {
            return (false, "Menores de idade não podem possuir receitas.", null);
        }

        var transacao = new Transacao
        {
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo!.Value,
            PessoaId = dto.PessoaId
        };

        var criada = await _transacaoRepository.AdicionarAsync(transacao, cancellationToken);

        return (true, null, new TransacaoResponseDto
        {
            Id = criada.Id,
            Descricao = criada.Descricao,
            Valor = criada.Valor,
            Tipo = (int)criada.Tipo,
            PessoaId = criada.PessoaId,
            PessoaNome = pessoa.Nome
        });
    }

    public async Task<List<TransacaoResponseDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var transacoes = await _transacaoRepository.ListarAsync(cancellationToken);

        return transacoes.Select(t => new TransacaoResponseDto
        {
            Id = t.Id,
            Descricao = t.Descricao,
            Valor = t.Valor,
            Tipo = (int)t.Tipo,
            PessoaId = t.PessoaId,
            PessoaNome = t.Pessoa?.Nome ?? string.Empty
        }).ToList();
    }

    public async Task<TransacaoResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id, cancellationToken);
        return transacao is null ? null : new TransacaoResponseDto
        {
            Id = transacao.Id,
            Descricao = transacao.Descricao,
            Valor = transacao.Valor,
            Tipo = (int)transacao.Tipo,
            PessoaId = transacao.PessoaId,
            PessoaNome = transacao.Pessoa?.Nome ?? string.Empty
        };
    }
}
