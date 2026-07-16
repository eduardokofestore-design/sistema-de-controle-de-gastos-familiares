using System.ComponentModel.DataAnnotations;

namespace ControleGastos.API.DTOs;

public class PessoaCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A idade é obrigatória.")]
    [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser maior ou igual a zero.")]
    public int? Idade { get; set; }
}
