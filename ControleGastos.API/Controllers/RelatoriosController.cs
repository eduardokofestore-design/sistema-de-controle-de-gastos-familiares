using ControleGastos.API.DTOs;
using ControleGastos.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleGastos.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly RelatorioService _service;

    public RelatoriosController(RelatorioService service)
    {
        _service = service;
    }

    /// <summary>Retorna receitas, despesas e saldo por pessoa e no total geral.</summary>
    [HttpGet("totais")]
    public async Task<ActionResult<RelatorioTotalDto>> ObterTotais(CancellationToken cancellationToken)
    {
        var relatorio = await _service.ObterTotaisAsync(cancellationToken);
        return Ok(relatorio);
    }
}
