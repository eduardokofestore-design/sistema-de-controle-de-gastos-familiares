using System.ComponentModel.DataAnnotations;

namespace ControleGastos.API.Models;

public class Pessoa
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser maior ou igual a zero.")]
    public int Idade { get; set; }

    public ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}
