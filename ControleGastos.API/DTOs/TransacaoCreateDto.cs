using System.ComponentModel.DataAnnotations;
using ControleGastos.API.Models;

namespace ControleGastos.API.DTOs;

public class TransacaoCreateDto
{
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    [MaxLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres.")]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [EnumDataType(typeof(TipoTransacao), ErrorMessage = "O tipo da transação é inválido.")]
    public TipoTransacao? Tipo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "O identificador da pessoa é inválido.")]
    public int PessoaId { get; set; }
}
