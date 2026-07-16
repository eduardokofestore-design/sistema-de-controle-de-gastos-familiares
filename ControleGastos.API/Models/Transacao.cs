using System.ComponentModel.DataAnnotations;

namespace ControleGastos.API.Models;

public class Transacao
{
    public int Id { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    public TipoTransacao Tipo { get; set; }

    public int PessoaId { get; set; }

    public Pessoa? Pessoa { get; set; }
}
