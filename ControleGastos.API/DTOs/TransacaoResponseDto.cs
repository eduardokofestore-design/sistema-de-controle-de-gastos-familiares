namespace ControleGastos.API.DTOs;

public class TransacaoResponseDto
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public int Tipo { get; set; }
    public int PessoaId { get; set; }
    public string PessoaNome { get; set; } = string.Empty;
}
