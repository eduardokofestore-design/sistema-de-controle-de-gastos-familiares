using ControleGastos.API.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransacoesController : ControllerBase
{
    private readonly TransacaoService _service;

    public TransacoesController(TransacaoService service)
    {
        _service = service;
    }

    /// <summary>Registra uma receita ou despesa para uma pessoa.</summary>
    /// <response code="201">Transação criada.</response>
    /// <response code="400">Dados inválidos ou receita para menor de idade.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpPost]
    public async Task<ActionResult<TransacaoResponseDto>> Criar([FromBody] TransacaoCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var (sucesso, mensagemErro, resultado) = await _service.CriarAsync(dto, cancellationToken);
        if (!sucesso)
        {
            if (mensagemErro == "A pessoa informada não existe.")
            {
                return NotFound(new { mensagem = mensagemErro });
            }

            return BadRequest(new { mensagem = mensagemErro });
        }

        return CreatedAtAction(nameof(ObterPorId), new { id = resultado!.Id }, resultado);
    }

    /// <summary>Lista todas as transações cadastradas.</summary>
    [HttpGet]
    public async Task<ActionResult<List<TransacaoResponseDto>>> Listar(CancellationToken cancellationToken)
    {
        var transacoes = await _service.ListarAsync(cancellationToken);
        return Ok(transacoes);
    }

    /// <summary>Obtém uma transação pelo identificador.</summary>
    /// <response code="404">Transação não encontrada.</response>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransacaoResponseDto>> ObterPorId(int id, CancellationToken cancellationToken)
    {
        var transacao = await _service.ObterPorIdAsync(id, cancellationToken);
        return transacao is null ? NotFound() : Ok(transacao);
    }
}
