using ControleGastos.API.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly PessoaService _service;

    public PessoasController(PessoaService service)
    {
        _service = service;
    }

    /// <summary>Cria uma pessoa.</summary>
    /// <response code="201">Pessoa criada.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    public async Task<ActionResult<PessoaResponseDto>> Criar([FromBody] PessoaCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var pessoa = await _service.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
    }

    /// <summary>Lista todas as pessoas cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<PessoaResponseDto>>> Listar(CancellationToken cancellationToken)
    {
        var pessoas = await _service.ListarAsync(cancellationToken);
        return Ok(pessoas);
    }

    /// <summary>Obtém uma pessoa pelo identificador.</summary>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PessoaResponseDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var pessoa = await _service.ObterPorIdAsync(id, cancellationToken);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }

    /// <summary>Exclui uma pessoa e todas as suas transações.</summary>
    /// <response code="204">Pessoa excluída.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
    {
        var removida = await _service.ExcluirAsync(id, cancellationToken);
        if (!removida)
        {
            return NotFound("A pessoa informada não existe.");
        }

        return NoContent();
    }
}
