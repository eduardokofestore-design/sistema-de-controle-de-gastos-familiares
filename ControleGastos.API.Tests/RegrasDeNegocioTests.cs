using ControleGastos.API.Data;
using ControleGastos.API.DTOs;
using ControleGastos.API.Models;
using ControleGastos.API.Repositories;
using ControleGastos.API.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ControleGastos.API.Tests;

public class RegrasDeNegocioTests
{
    [Fact]
    public async Task NaoPermiteReceitaParaMenorDeIdade()
    {
        await using var fixture = await CriarFixtureAsync();
        var pessoa = await fixture.PessoaService.CriarAsync(new PessoaCreateDto { Nome = "Ana", Idade = 17 });

        var resultado = await fixture.TransacaoService.CriarAsync(new TransacaoCreateDto
        {
            Descricao = "Mesada",
            Valor = 50m,
            Tipo = TipoTransacao.Receita,
            PessoaId = pessoa.Id
        });

        Assert.False(resultado.Sucesso);
        Assert.Equal("Menores de idade não podem possuir receitas.", resultado.MensagemErro);
    }

    [Fact]
    public async Task ExcluirPessoaRemoveSuasTransacoes()
    {
        await using var fixture = await CriarFixtureAsync();
        var pessoa = await fixture.PessoaService.CriarAsync(new PessoaCreateDto { Nome = "Bruno", Idade = 25 });
        await fixture.TransacaoService.CriarAsync(new TransacaoCreateDto
        {
            Descricao = "Salário",
            Valor = 100m,
            Tipo = TipoTransacao.Receita,
            PessoaId = pessoa.Id
        });

        var removida = await fixture.PessoaService.ExcluirAsync(pessoa.Id);

        Assert.True(removida);
        Assert.Empty(await fixture.Context.Transacoes.ToListAsync());
    }

    [Fact]
    public async Task RelatorioCalculaTotaisPorPessoaEGeral()
    {
        await using var fixture = await CriarFixtureAsync();
        var pessoa = await fixture.PessoaService.CriarAsync(new PessoaCreateDto { Nome = "Carla", Idade = 30 });
        await fixture.TransacaoService.CriarAsync(new TransacaoCreateDto { Descricao = "Freelance", Valor = 200m, Tipo = TipoTransacao.Receita, PessoaId = pessoa.Id });
        await fixture.TransacaoService.CriarAsync(new TransacaoCreateDto { Descricao = "Mercado", Valor = 75m, Tipo = TipoTransacao.Despesa, PessoaId = pessoa.Id });

        var relatorio = await fixture.RelatorioService.ObterTotaisAsync();
        var totalDaPessoa = Assert.Single(relatorio.Pessoas);

        Assert.Equal(pessoa.Id, totalDaPessoa.PessoaId);
        Assert.Equal(200m, totalDaPessoa.TotalReceitas);
        Assert.Equal(75m, totalDaPessoa.TotalDespesas);
        Assert.Equal(125m, totalDaPessoa.Saldo);
        Assert.Equal(125m, relatorio.SaldoGeral);
    }

    private static async Task<Fixture> CriarFixtureAsync()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var pessoaRepository = new PessoaRepository(context);
        var transacaoRepository = new TransacaoRepository(context);
        return new Fixture(context, connection, new PessoaService(pessoaRepository), new TransacaoService(transacaoRepository, pessoaRepository), new RelatorioService(pessoaRepository, transacaoRepository));
    }

    private sealed class Fixture(
        AppDbContext context,
        SqliteConnection connection,
        PessoaService pessoaService,
        TransacaoService transacaoService,
        RelatorioService relatorioService) : IAsyncDisposable
    {
        public AppDbContext Context { get; } = context;
        public PessoaService PessoaService { get; } = pessoaService;
        public TransacaoService TransacaoService { get; } = transacaoService;
        public RelatorioService RelatorioService { get; } = relatorioService;

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await connection.DisposeAsync();
        }
    }
}
