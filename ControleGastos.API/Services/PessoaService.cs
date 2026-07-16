using ControleGastos.API.DTOs;
using ControleGastos.API.Models;
using ControleGastos.API.Repositories;

namespace ControleGastos.API.Services;

public class PessoaService
{
    private readonly PessoaRepository _repository;

    public PessoaService(PessoaRepository repository)
    {
        _repository = repository;
    }

    public async Task<PessoaResponseDto> CriarAsync(PessoaCreateDto dto, CancellationToken cancellationToken = default)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade!.Value
        };

        var criada = await _repository.AdicionarAsync(pessoa, cancellationToken);

        return new PessoaResponseDto
        {
            Id = criada.Id,
            Nome = criada.Nome,
            Idade = criada.Idade
        };
    }

    public async Task<List<PessoaResponseDto>> ListarAsync(CancellationToken cancellationToken = default)
    {
        var pessoas = await _repository.ListarAsync(cancellationToken);

        return pessoas.Select(p => new PessoaResponseDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Idade = p.Idade
        }).ToList();
    }

    public async Task<PessoaResponseDto?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var pessoa = await _repository.ObterPorIdSomenteLeituraAsync(id, cancellationToken);
        return pessoa is null ? null : new PessoaResponseDto
        {
            Id = pessoa.Id,
            Nome = pessoa.Nome,
            Idade = pessoa.Idade
        };
    }

    public async Task<bool> ExcluirAsync(int id, CancellationToken cancellationToken = default)
    {
        var pessoa = await _repository.ObterPorIdAsync(id, cancellationToken);
        if (pessoa is null)
        {
            return false;
        }

        await _repository.RemoverAsync(pessoa, cancellationToken);
        return true;
    }
}
