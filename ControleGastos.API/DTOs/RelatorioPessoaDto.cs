namespace ControleGastos.API.DTOs;

public class RelatorioPessoaDto
{
    public int PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal TotalReceitas { get; set; }
    public decimal TotalDespesas { get; set; }
    public decimal Saldo { get; set; }
}
