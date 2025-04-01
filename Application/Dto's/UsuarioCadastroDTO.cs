using System.ComponentModel.DataAnnotations;

namespace PicPaySimplificado.Application.DTOs
{
    public class UsuarioCadastroDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ é obrigatório.")]
        [RegularExpression(@"^\d{11}|\d{14}$", ErrorMessage = "CPF deve ter 11 dígitos e CNPJ deve ter 14.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Tipo de usuário é obrigatório.")]
        public string TipoUsuario { get; set; } // "COMUM" ou "LOJISTA"
    }
}
