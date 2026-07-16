namespace ControleGastos.API.DTOs;

public class RelatorioTotalDto
{
    public List<RelatorioPessoaDto> Pessoas { get; set; } = new();
    public decimal TotalReceitasGeral { get; set; }
    public decimal TotalDespesasGeral { get; set; }
    public decimal SaldoGeral { get; set; }
}
